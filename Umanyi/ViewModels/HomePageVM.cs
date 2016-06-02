using Helper;
using Helper.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "None")]
    public class HomePageVM : ViewModelBase
    {
        private List<ItemInfo> filesInfo;
        private Window window;
        private bool showImageFeed;
        private bool showVideoFeed;
        private BitmapImage imageSource;
        private Uri videoSource;
        DispatcherTimer t;
        int currIndex = 0;
        private bool isMuted;
        private TimeSpan currentMediaDuration;
        public HomePageVM()
        {
            InitVars();
            CreateCommands();            
        }

        protected async override void InitVars()
        {
            ShowVideoFeed = false;
            ShowImageFeed = true;
            ImageSource = GetDefaultImage();
            filesInfo = await LoadFileInfos();
            if (filesInfo.Count>0)
            t = new DispatcherTimer(new TimeSpan(0,0,15), DispatcherPriority.DataBind,LoadFeeds,Dispatcher.CurrentDispatcher);
            
            t.Start();
            PropertyChanged += (o, e1) =>
                {
                    if (e1.PropertyName == "VideoSource" && VideoSource != null && ShowVideoFeed)
                        if (PlayAction != null)
                            PlayAction.Invoke();
                };
        }

        private Task<List<ItemInfo>> LoadFileInfos()
        {
            return Task.Run<List<ItemInfo>>(() =>
                {
                    List<ItemInfo> temp = new List<ItemInfo>();

                    string selectStr = "SELECT GalleryID, Name, DATALENGTH(Data) FROM [Institution].[Gallery]";
            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            ItemInfo gim;
            foreach (DataRow dtr in dt.Rows)
            {
                gim = new ItemInfo();
                gim.MediaID = int.Parse(dtr[0].ToString());
                gim.Name = dtr[1].ToString();
                gim.Size = long.Parse(dtr[2].ToString());
                temp.Add(gim);
            }
                    return temp;
                });
        }



        private BitmapImage GetDefaultImage()
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(@"pack://application:,,,/UmanyiSMS;component/Resources/bg.jpg",UriKind.Absolute);
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            return img;
        }

        protected override void CreateCommands()
        {
            LogoutCommand = new RelayCommand(o =>
            {
                if (MessageBoxResult.Yes== MessageBox.Show("Are you sure you would like to logout?","Info", 
                    MessageBoxButton.YesNo, MessageBoxImage.Information))
                {
                    App.Restart();
                }

            }, o => true);

            PauseCommand=new RelayCommand(o =>
            {
               t.Stop();
                if (PauseAction!=null)
                    PauseAction.Invoke();
            }, o => t!=null&& t.IsEnabled);

            ResumeCommand = new RelayCommand(o =>
            {
               t.Start();
                if (PlayAction!=null)
                    PlayAction.Invoke();
            }, o => t != null && !t.IsEnabled);

            MuteCommand = new RelayCommand(o =>
            {
                IsMuted = true;
            }, o => showVideoFeed && t != null && t.IsEnabled);

            UnmuteCommand = new RelayCommand(o =>
            {
                IsMuted = false;
            }, o => showVideoFeed && t != null && t.IsEnabled && isMuted);

            NextCommand = new RelayCommand(o =>
            {
                currIndex++;
                LoadFeeds(null);
            }, o => true);
        }

        private void LoadFeeds(object state, EventArgs e)
        {
            LoadFeeds(state);
        }

        internal void LoadFeeds(object state)
        {
            if (currIndex >= filesInfo.Count)
                currIndex = 0;
            string path = Task.Run<string>(async () => { return await LoadMediaData(filesInfo[currIndex]); }).Result;
            if (NextMediaIsImage(path))
            {
                if (!ShowImageFeed)
                {
                    ShowImageFeed = true;
                    ShowVideoFeed = false;
                }
                ImageSource = GetNextImage(path);
                t.Interval = new TimeSpan(0,0,15);
                VideoSource = null;
            }
            else
            {
                if (!ShowVideoFeed)
                {
                    ShowVideoFeed = true;
                    ShowImageFeed = false;
                }
                VideoSource = GetNextMedia(path);
                this.PropertyChanged += (o, e) =>
                    {
                        if (e.PropertyName=="CurrentMediaDuration")
                            t.Interval = CurrentMediaDuration;
                    };
                
            }
            currIndex++;
        }

        private Task<string> LoadMediaData(ItemInfo p)
        {
            return Task.Run<string>(() =>
                {
                    string path = Path.GetTempPath() + p.Name;
                    bool exists = File.Exists(path) && (new FileInfo(path).Length == p.Size);
                    if (exists)
                    return path;

                    string selectStr = "SELECT Data FROM [Institution].[Gallery] WHERE GalleryID="+p.MediaID;
                    byte[] bytes = (byte[])DataAccessHelper.ExecuteObjectScalar(selectStr,true);
                    using (var memStream = new MemoryStream(bytes))
                    {
                        if (!exists)
                            using (var fileStream = File.OpenWrite(path))
                            {
                                memStream.CopyTo(fileStream);
                                fileStream.Flush();
                            }
                        memStream.Flush();
                    }
                    return path;
                });
        }

        private Uri GetNextMedia(string path)
        {
           return new Uri(path);
        }

        private BitmapImage GetNextImage(string path)
        {
            try
            {
                var bt=new BitmapImage(new Uri(path));
                return bt;
            }
            catch
            {
                return null;
            }
        }
            
        

        private bool NextMediaIsImage(string path)
        {
            if (new FileInfo(path).Length > 20000000)
                return false;
            try
            {
                new BitmapImage(new Uri(path));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public Action PlayAction
        {
            get;
            set;
        }

        public Action PauseAction
        {
            get;
            set;
        }

        public ICommand LogoutCommand
        {
            get;
            private set;
        }

        public ICommand PauseCommand
        {
            get;
            private set;
        }

        public ICommand ResumeCommand
        {
            get;
            private set;
        }

        public ICommand MuteCommand
        {
            get;
            private set;
        }

        public ICommand UnmuteCommand
        {
            get;
            private set;
        }

        public ICommand NextCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            
        }

        public bool IsMuted
        {
            get { return isMuted; }
            set
            {
                if (value != this.isMuted)
                {
                    this.isMuted = value;
                    NotifyPropertyChanged("IsMuted");
                }
            }
        }

        public bool ShowVideoFeed
        {
            get { return showVideoFeed; }
            set
            {
                if (value != this.showVideoFeed)
                {
                    this.showVideoFeed = value;
                    NotifyPropertyChanged("ShowVideoFeed");
                }
            }
        }

        public bool ShowImageFeed
        {
            get { return showImageFeed; }
            set
            {
                if (value != this.showImageFeed)
                {
                    this.showImageFeed = value;
                    NotifyPropertyChanged("ShowImageFeed");
                }
            }
        }

        public BitmapImage ImageSource
        {
            get { return imageSource; }
            private set
            {
                if (value != this.imageSource)
                {
                    this.imageSource = value;
                    NotifyPropertyChanged("ImageSource");
                }
            }
        }

        public Uri VideoSource
        {
            get { return videoSource; }
            private set
            {
                if (value != this.videoSource)
                {
                    this.videoSource = value;
                    NotifyPropertyChanged("VideoSource");
                }
            }
        }

        internal void SetWindow(Window window)
        {
            this.window = window;
        }

        class ItemInfo
        {
            public int MediaID { get; set; }

            public long Size { get; set; }

            public string Name { get; set; }
        }



        public TimeSpan CurrentMediaDuration
        {
            get { return currentMediaDuration; }
            set
            {
                if (value != this.currentMediaDuration)
                {
                    this.currentMediaDuration = value;
                    NotifyPropertyChanged("CurrentMediaDuration");
                }
            }
        }
    }
}
