using Helper;
using Helper.Models;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Collections.Generic;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class StudentListVM : ViewModelBase
    {
        ObservableCollection<StudentListModel> allStudents;
        string searchText="";
        CollectionViewSource collViewSource;
        bool showCleared;
        bool showTransferred;
        public StudentListVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void CreateCommands()
        {
            StudentDetailsCommand = new RelayCommand(o =>
            {
                if (OpenTaskWindowAction != null)
                    OpenTaskWindowAction.Invoke(new StudentDetailsVM(o as StudentModel));
            }, o => (o is StudentModel));
            RefreshCommand = new RelayCommand(o => { Reset(); }, o => true);

            SetNewClassCommand = new RelayCommand(o =>
            {/*
                GalleryItemModel gim = o as GalleryItemModel;
                if (o == null)
                {
                    MessageBox.Show("No Student Selected.","Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return;
                }
                //else DataAccess.SetNewStudentsClassAsync(GetSelectedItems(),)*/
            }, o => GetSelectedItemsCount()>0);

            SetTransferredCommand = new RelayCommand(o =>
            {
                /*bool succ = DataAccess.SaveNewStudentTransfersAsync(GetSelectedItems());
                if (succ)
                    MessageBox.Show("Successfully saved details.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Could Not save details.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                */
            }, o => GetSelectedItemsCount() > 0);

            SetClearedCommand = new RelayCommand(o =>
            {/*
                bool succ = DataAccess.SaveNewStudentClearancesAsync(GetSelectedItems());
                if (succ)
                    MessageBox.Show("Successfully saved details.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Could Not save details.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
              */
            }, o => GetSelectedItemsCount() > 0);

            SetActiveCommand = new RelayCommand(o =>
            {
                /*
                bool succ = DataAccess.SaveActiveStudentsActiveAsync(GetSelectedItems());
                if (succ)
                    MessageBox.Show("Successfully saved details.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Could Not save details.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);*/
            }, o => GetSelectedItemsCount() > 0);

            ShowAllCommand = new RelayCommand(o =>
            {/*
                GalleryItemModel gim = o as GalleryItemModel;
                if (o == null)
                {
                    MessageBox.Show("No Student Selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return;
                }
                else MessageBox.Show(GetSelectedItemsCount() + "");*/
            }, o => GetSelectedItemsCount() > 0);

            ShowActiveOnlyCommand = new RelayCommand(o =>
            {/*
                GalleryItemModel gim = o as GalleryItemModel;
                if (o == null)
                {
                    MessageBox.Show("No Student Selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return;
                }
                else MessageBox.Show(GetSelectedItemsCount() + "");*/
            }, o => GetSelectedItemsCount() > 0);

            ShowClearedCommand = new RelayCommand(o =>
            {
                /*GalleryItemModel gim = o as GalleryItemModel;
                if (o == null)
                {
                    MessageBox.Show("No Student Selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return;
                }
                else MessageBox.Show(GetSelectedItemsCount() + "");*/
            }, o => GetSelectedItemsCount() > 0);

            ShowTransferredOnlyCommand = new RelayCommand(o =>
            {
                /*
                GalleryItemModel gim = o as GalleryItemModel;
                if (o == null)
                {
                    MessageBox.Show("No Student Selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return;
                }
                else MessageBox.Show(GetSelectedItemsCount() + "");*/
            }, o => GetSelectedItemsCount() > 0);

            IncludeClearedCommand = new RelayCommand(o =>
            {
                /*GalleryItemModel gim = o as GalleryItemModel;
                if (o == null)
                {
                    MessageBox.Show("No Student Selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return;
                }
                else MessageBox.Show(GetSelectedItemsCount() + "");*/
            }, o => true);

            IncludeTransferredCommand = new RelayCommand(o =>
            {
                /*GalleryItemModel gim = o as GalleryItemModel;
                if (o == null)
                {
                    MessageBox.Show("No Student Selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning); return;
                }
                else MessageBox.Show(GetSelectedItemsCount() + "");*/
            }, o => true);
        }

        protected async override void InitVars()
        {
            Title = "STUDENT LIST";
            collViewSource = new CollectionViewSource();
            SearchText = "";
            allStudents = await DataAccess.GetAllStudentsListAsync();
            CollViewSource.Source = allStudents;
            ShowTransferred = true;
            ShowCleared = true;
            PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName=="IsActive")
                        if (IsActive)
                        {
                            allStudents = await DataAccess.GetAllStudentsListAsync();
                            CollViewSource.Source = allStudents;
                        }
                };
        }

        private int GetSelectedItemsCount()
        {
            int i=0;
            ObservableCollection<StudentListModel> l = (ObservableCollection<StudentListModel>)collViewSource.Source;
            foreach (StudentListModel slm in l)
                if (slm.IsSelected)
                    i++;
            return i;
        }

        private ObservableCollection<StudentListModel> GetSelectedItems()
        {
            ObservableCollection<StudentListModel> temp = new ObservableCollection<StudentListModel>();
            ObservableCollection<StudentListModel> l = (ObservableCollection<StudentListModel>)collViewSource.Source;
            foreach (StudentListModel slm in l)
                if (slm.IsSelected)
                {
                    StudentListModel df = new StudentListModel();
                    df.CopyFrom(slm);
                    temp.Add(df);
                }
            return temp;
        }

        public async override void Reset()
        {
            allStudents = await DataAccess.GetAllStudentsListAsync();
            CollViewSource.Source = allStudents;
            searchText = "";
        }

        public Action<ViewModelBase> OpenTaskWindowAction
        { get; internal set; }

        public CollectionViewSource CollViewSource
        {
            get
            {
                return collViewSource;
            }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    searchText = value;
                    RenewFilter();
                    if (CollViewSource.View != null)
                        CollViewSource.View.Refresh();
                }
                else
                {
                    if (searchText != null)
                        RemoveFilter();
                    if (CollViewSource.View != null)
                        CollViewSource.View.Refresh();
                }
            }
        }

        public bool ShowTransferred
        {
            get { return showTransferred; }
            set
            {
                if (showTransferred != value)
                {
                    showTransferred = value;
                    NotifyPropertyChanged("ShowTransferred");
                    RenewFilter();
                }
            }
        }

        public bool ShowCleared
        {
            get { return showCleared; }
            set
            {
                if (showCleared != value)
                {
                    showCleared = value;
                    NotifyPropertyChanged("ShowCleared");
                    RenewFilter();
                }
            }
        }
        
        private void RenewFilter()
        {
            RemoveFilter();
            CollViewSource.Filter += new FilterEventHandler(Filter);
        }
        private void RemoveFilter()
        {
            CollViewSource.Filter -= new FilterEventHandler(Filter);
        }

        private void Filter(object sender, FilterEventArgs e)
        {
            e.Accepted = false;
            var src = e.Item as StudentListModel;
            if (src == null)
            {
                e.Accepted = false;
            }
            else
            {
                if (DataAccess.SearchAllStudentProperties(src, SearchText))
                    e.Accepted = (!src.IsCleared | showCleared) &&
                        (!src.IsTransferred | showTransferred);
                else e.Accepted = false;
            }
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }

        public ICommand StudentDetailsCommand
        {
            get;
            private set;
        }

        public ICommand SetNewClassCommand
        {
            get;
            private set;
        }
        public ICommand SetActiveCommand
        {
            get;
            private set;
        }
        public ICommand SetTransferredCommand
        {
            get;
            private set;
        }
        public ICommand SetClearedCommand
        {
            get;
            private set;
        }
        public ICommand ShowClearedCommand
        {
            get;
            private set;
        }

        public ICommand ShowTransferredOnlyCommand
        {
            get;
            private set;
        }

        public ICommand ShowAllCommand
        {
            get;
            private set;
        }

        public ICommand ShowActiveOnlyCommand
        {
            get;
            private set;
        }


        public ICommand IncludeTransferredCommand
        {
            get;
            private set;
        }

        public ICommand IncludeClearedCommand
        {
            get;
            private set;
        }

        public ICommand ExportToExcelCommand
        {
            get;
            private set;
        }
        
            
                    
                        
                        
                            
                            
                                
                                
    }
}
