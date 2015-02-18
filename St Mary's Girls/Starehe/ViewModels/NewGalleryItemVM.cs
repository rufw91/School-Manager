using Helper;
using Helper.Converters;
using Helper.Models;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class NewGalleryItemVM: ViewModelBase
    {
        private ObservableCollection<GalleryItemModel> entries;
        private Process process;
        GalleryItemModel currentlyPlayingItem;
        GalleryItemModel selectedItem;
        private bool showMenu;
        private bool canPlay;
        public NewGalleryItemVM()
        {
            InitVars();
            CreateCommands();
        }

        protected override void InitVars()
        {
            CanPlay = true;
            Title = "ADD ITEM(S)";
            Entries = new ObservableCollection<GalleryItemModel>();
            PropertyChanged += (o, e) =>
                {
                    if ((e.PropertyName == "SelectedItem") || (e.PropertyName == "CurrentlyPlayingItem"))
                        ShowMenu = CompareSelectedAndPlaying();
                };
        }
        
        protected override void CreateCommands()
        {
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
            }, o => !IsBusy&&CanPlay);

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
        

            ClearCommand = new RelayCommand(o =>
                {
                    entries.Clear();
                }, o => true
            );
            BrowseCommand = new RelayCommand(async o =>
            {
                string path = FileHelper.BrowseMediaAsString();
                if (string.IsNullOrWhiteSpace(path))
                    return;
                GalleryItemModel gm = await GetGalleryItemFromPath(path);

                if (TotalSize() <= 1073741824)
                    if (gm.Size <= 104857600)
                        entries.Add(gm);
                    else
                        MessageBox.Show("File is too Large!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                    MessageBox.Show("Limit reached!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, o => true);

            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataAccess.SaveNewGalleryItemsAsync(entries);
                IsBusy = false;
                if (succ)
                    Reset();
            }, o => !IsBusy && CanSave());
            SaveAndShareCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataAccess.SaveNewGalleryItemsAsync(entries);
                IsBusy = false;
                if (succ)
                    Reset();
            }, o => !IsBusy && CanSave()&&false);
        }

        private Task<GalleryItemModel> GetGalleryItemFromPath(string path)
        {
            return Task.Run<GalleryItemModel>(() =>
            {
                var v = new FileInfo(path);
                GalleryItemModel gim = new GalleryItemModel();
                gim.Path = path;
                gim.Name = v.Name;
                gim.Thumbnail =ImageConverters.BitmapSourceToByteArray(ShellFile.FromFilePath(path).Thumbnail.MediumBitmapSource);
                gim.Size = new FileInfo(path).Length;
                return gim;
            });
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

        private long TotalSize()
        {
            long totSize = 0;
            foreach (GalleryItemModel gm in entries)
                totSize += gm.Size;
            return totSize;
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
            return entries.Count>0&&itemsOK;
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

        public ObservableCollection<GalleryItemModel> Entries
        {
            get { return entries; }
            private set
            {
                if (entries!=value)
                {
                    entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
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

        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public ICommand SaveAndShareCommand
        {
            get;
            private set;
        }

        public ICommand ClearCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            Entries = new ObservableCollection<GalleryItemModel>();
        }

    }
}
