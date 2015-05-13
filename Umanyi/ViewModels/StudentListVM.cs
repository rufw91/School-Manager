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

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class StudentListVM : ViewModelBase
    {
        ObservableCollection<StudentListModel> allStudents;
        string searchText="";
        CollectionViewSource collViewSource;
        bool showCleared;
        bool showInactive;
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
                    OpenTaskWindowAction.Invoke(new StudentDetailsVM(o as StudentListModel));
            }, o => (o is StudentListModel));
            RefreshCommand = new RelayCommand(o => { Reset(); }, o => true);

        }

        protected async override void InitVars()
        {
            Title = "STUDENT LIST";
            collViewSource = new CollectionViewSource();
            SearchText = "";
            allStudents = await DataAccess.GetAllStudentsListAsync();
            CollViewSource.Source = allStudents;
            ShowInactive = false;
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

        public bool ShowInactive
        {
            get { return showInactive; }
            set
            {
                if (showInactive != value)
                {
                    showInactive = value;
                    NotifyPropertyChanged("ShowInactive");
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
                    e.Accepted = (src.IsActive | !showInactive);
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

        
        public ICommand ExportToExcelCommand
        {
            get;
            private set;
        }
        
            
                    
                        
                        
                            
                            
                                
                                
    }
}
