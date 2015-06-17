using Helper;
using Helper.Converters;
using Helper.Models;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class RecentGalleryItemsVM: ViewModelBase, IDisposable
    {    
        private ObservableImmutableList<GalleryItemModel> entries;
        private Process process;
        GalleryItemModel currentlyPlayingItem;
        GalleryItemModel selectedItem;
        private bool showMenu;
        private bool canPlay;
        TempFileCollection tfx;
        public RecentGalleryItemsVM()
        {
            InitVars();
            CreateCommands();
        }

        ~RecentGalleryItemsVM()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (tfx != null)
                tfx.Delete();
        }

        protected async override void InitVars()
        {
            IsBusy = true;
            TFC = new TempFileCollection(Path.GetTempPath(), true);
            CanPlay = true;
            Title = "RECENT ITEMS";
            PropertyChanged += (o, e) =>
            {
                if ((e.PropertyName == "SelectedItem") || (e.PropertyName == "CurrentlyPlayingItem"))
                    ShowMenu = CompareSelectedAndPlaying();
            };
            Entries = new ObservableImmutableList<GalleryItemModel>();
            await GetRecentGalleryItems();
            IsBusy = false; 
        }

        private async Task GetRecentGalleryItems()
        {
            string selectStr = "SELECT TOP 50 GalleryID, Name, DATALENGTH(Data) FROM [Institution].[Gallery]";
            DataTable dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            List<GalleryItemModel> existingFiles = new List<GalleryItemModel>();
            string notInList = "";
            GalleryItemModel gim;
            bool exists;
            long size;
            foreach (DataRow dtr in dt.Rows)
            {
                gim = new GalleryItemModel();
                gim.GalleryID = int.Parse(dtr[0].ToString());
                gim.Name = dtr[1].ToString();
                gim.Path = Path.GetTempPath() + gim.Name;
                size = long.Parse(dtr[2].ToString());
                exists = File.Exists(gim.Path) && (new FileInfo(gim.Path).Length == size);
                if (exists)
                {
                    existingFiles.Add(gim);
                    if (string.IsNullOrWhiteSpace(notInList))
                        notInList = " WHERE GalleryID NOT IN(" + gim.GalleryID;
                    else
                        notInList += "," + gim.GalleryID;
                }
            }
            notInList = (string.IsNullOrWhiteSpace(notInList)) ? notInList : notInList + ")";
            Task t1 = Task.Run(() =>
            {
                selectStr = "SELECT TOP 50 GalleryID, Data, Name FROM [Institution].[Gallery]" + notInList;
                dt = DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);

                foreach (DataRow dtr in dt.Rows)
                {
                    gim = new GalleryItemModel();
                    gim.GalleryID = int.Parse(dtr[0].ToString());
                    gim.Data = (byte[])dtr[1];
                    gim.Name = dtr[2].ToString();
                    gim.Path = Path.GetTempPath() + gim.Name;
                    gim.Size = gim.Data.Length;
                    UpdateEntry(gim, TFC);
                    gim.Data = null;
                    entries.Add(gim);
                }
            });

            Task t2 = Task.Run(() =>
                {
                    foreach(GalleryItemModel item in existingFiles)
                    {
                        item.Data = File.ReadAllBytes(item.Path);
                        try
                        {
                            item.Thumbnail = ImageConverters.BitmapSourceToByteArray(ShellFile.FromFilePath(item.Path).Thumbnail.ExtraLargeBitmapSource);
                        }
                        catch { }
                        item.Size = item.Data.Length;
                        item.Data = null;
                        entries.Add(item);
                    }
                });
            await Task.WhenAll(t1, t2);
        }

        private void UpdateEntry(GalleryItemModel item, TempFileCollection tfc)
        {            
            tfc.AddFile(item.Path, true);

            using (var memStream = new MemoryStream(item.Data))
            {
                bool exists = File.Exists(item.Path) && (new FileInfo(item.Path).Length == item.Size);
                if (!exists)
                    using (var fileStream = File.OpenWrite(item.Path))
                    {
                        memStream.CopyTo(fileStream);
                    }
            }
            item.Thumbnail = ImageConverters.BitmapSourceToByteArray(ShellFile.FromFilePath(item.Path).Thumbnail.ExtraLargeBitmapSource);
        }

        protected override void CreateCommands()
        {
            ShareCommand = new RelayCommand(o =>
            {
            }, o => !IsBusy && CanPlay);

            RefreshCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                entries.Clear();
                await GetRecentGalleryItems();
                IsBusy = false; 
            },o=>true);

            OpenCommand = new RelayCommand(async o =>
            {
                GalleryItemModel gim = o as GalleryItemModel;
                if (o == null)
                {
                    MessageBox.Show("Item is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning); return;
                }
                CurrentlyPlayingItem = gim;
                try
                {
                    process = Process.Start(gim.Path);
                    IsBusy = true;
                    CanPlay = false;
                    await Task.Run(() => Thread.Sleep(new TimeSpan(0, 0, 5))); 
                    IsBusy = false;
                }
                catch { }
            }, o => !IsBusy && CanPlay);

            StopCommand = new RelayCommand(o =>
            {
                try
                {
                    if (process != null)
                        if (!process.HasExited)
                            process.Kill();
                    CanPlay = true;
                    CurrentlyPlayingItem = null;
                }
                catch { }
            }, o => !CanPlay);
        }

        private bool CanStop()
        {
            if (process == null)
            {
                CanPlay = true;
                return false;
            }

            if (process.HasExited)
            {
                CanPlay = true;
                return false;
            }
            return true;
        }

        public bool CanPlay
        {
            get { return canPlay; }
            set
            {
                if (value != canPlay)
                {
                    canPlay = value;
                    NotifyPropertyChanged("CanPlay");
                }
            }
        }

        private bool CanSave()
        {
            bool itemsOK = true;
            foreach (GalleryItemModel gim in entries)
            {
                if ((string.IsNullOrWhiteSpace(gim.Path) || (gim.Size > 104857600)))
                    itemsOK = false;
                break;
            }
            return entries.Count > 0 && itemsOK;
        }

        private bool CompareSelectedAndPlaying()
        {
            if ((selectedItem == null) && (currentlyPlayingItem == null))
                return true;
            if ((selectedItem != null) && (currentlyPlayingItem != null))
            {
                return (selectedItem.Size == currentlyPlayingItem.Size) && (selectedItem.Path == currentlyPlayingItem.Path);
            }
            if ((selectedItem != null) && (currentlyPlayingItem == null))
                return true;
            return false;
        }

        public GalleryItemModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                NotifyPropertyChanged("SelectedItem");
            }
        }

        public GalleryItemModel CurrentlyPlayingItem
        {
            get { return currentlyPlayingItem; }
            set
            {
                currentlyPlayingItem = value;
                NotifyPropertyChanged("CurrentlyPlayingItem");
            }
        }

        public bool ShowMenu
        {
            get { return showMenu; }
            private set
            {
                if (showMenu != value)
                {
                    showMenu = value;
                    NotifyPropertyChanged("ShowMenu");
                }
            }
        }

        private TempFileCollection TFC
        {
            get { return tfx; }
            set { tfx = value; }
        }

        public ObservableImmutableList<GalleryItemModel> Entries
        {
            get { return entries; }
            private set
            {
                if (entries != value)
                {
                    entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }

        public ICommand ShareCommand
        {
            get;
            private set;
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }
        
        public ICommand OpenCommand
        {
            get;
            private set;
        }

        public ICommand StopCommand
        {
            get;
            private set;
        }

        public ICommand BrowseCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
        }

    }
}
