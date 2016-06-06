using Helper;
using Helper.Helpers;
using Helper.Models;
using UmanyiSMS.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class MainWindowVM : ViewModelBase    
    {
        ViewModelBase source;
        ObservableCollection<ViewModelBase> backJournal;
        bool navigatingBack;
        public MainWindowVM() 
        {
            InitVars();
            CreateCommands();            
        }

        public ViewModelBase Source
        {
            get { return this.source; }

            set
            {
                if (value != this.source)
                {                   
                    this.source = value;                    
                    NotifyPropertyChanged("Source");
                    if (!navigatingBack)
                        BackJournal.Add(source);
                }
            }
        }
        
        public ObservableCollection<ViewModelBase> BackJournal
        { get { return backJournal; } }

        protected override void InitVars()
        {
            navigatingBack = false;
            backJournal = new ObservableCollection<ViewModelBase>();
            Source = new HomePageVM();
        }

        protected override void CreateCommands()
        {
            DisplayAcademicReportCommand = new RelayCommand(o =>
            {
                this.Source = new AcademicReportVM();
            }, o => true);

            DisplayStudentsReportCommand = new RelayCommand(o =>
            {
                this.Source = new StudentsReportVM();
            }, o => true);

            DisplayStaffReportCommand = new RelayCommand(o =>
            {
                this.Source = new StaffReportVM();
            }, o => true);

            DisplayFinanceReportCommand = new RelayCommand(o =>
            {
                this.Source = new FinanceReportVM();
            }, o => true);

            DisplayBoardingReportCommand = new RelayCommand(o =>
            {
                this.Source = new BoardingReportVM();
            }, o => true);
                        
            DisplayLibraryReportCommand = new RelayCommand(o =>
            {
                this.Source = new LibraryReportVM();
            }, o => true);

            DisplayEventsReportCommand = new RelayCommand(o =>
            {
                this.Source = new EventsReportVM();
            }, o => true);

            DisplayDisciplineDetailCommand = new RelayCommand(o =>
            {
                this.Source = new DisciplineDetailsVM((DisciplineModel)o);
            }, o => (o is DisciplineModel));
            AboutCommand = new RelayCommand(o =>
            {
                if (AboutAction != null)
                    AboutAction.Invoke();
            }, o => true);

            DisplayStudentDetailCommand = new RelayCommand(o =>
            {
                this.Source = new StudentDetailsVM((StudentListModel)o);
            }, o => (o is StudentListModel));

            DisplayEventDetailCommand = new RelayCommand(o =>
            {
                this.Source = new EventDetailsVM() { NewEvent = (EventModel)o };
            }, o => (o is EventModel));

            DisplayStaffDetailCommand = new RelayCommand(o =>
            {
                this.Source = new StaffDetailsVM((StaffModel)o);
            }, o => (o is StaffModel));

            DisplayCustomModulesCommand = new RelayCommand(o =>
            {
                this.Source = new CustomModulesVM();
            }, o => true);

            DisplayAccountsCommand = new RelayCommand(o =>
            {
                this.Source = new AccountsVM();
            }, o => true);

            DisplayStudentsCommand = new RelayCommand(o =>
            {
                this.Source = new StudentsVM();
            }, o => true);

            DisplayStaffCommand = new RelayCommand(o =>
            {
                this.Source = new StaffVM();
            }, o => true);

            DisplayFeesCommand = new RelayCommand(o =>
            {
                this.Source = new FinanceVM();
            }, o => true);

            DisplayInstitutionCommand = new RelayCommand(o =>
            {
                this.Source = new InstitutionVM();
            }, o => true);
            
            DisplayExamsCommand = new RelayCommand(o =>
            {
                this.Source = new ExamsVM();
            }, o => true);

            DisplayLibraryCommand = new RelayCommand(o =>
            {
                this.Source = new LibraryVM();
            }, o => true);

            DisplayGalleryCommand = new RelayCommand(o =>
            {
                this.Source = new GalleryVM();
            }, o => true);

            DisplaySettingsCommand = new RelayCommand(o =>
            {
                this.Source = new SettingsVM();
            }, o => true);

            DisplayInventoryMgmtCommand = new RelayCommand(o =>
            {
                this.Source = new InventoryMgmtVM();
            }, o => true);

            DisplayBoardingCommand = new RelayCommand(o =>
            {
                this.Source = new ReportsVM();
            }, o => true);

            DisplayPayrollCommand = new RelayCommand(o =>
            {
                this.Source = new PayrollVM();
            }, o => true);

            #region Inventory Mgmt
            DisplayItemsCommand = new RelayCommand(o =>
            {
                this.Source = new ItemsVM();
            }, o => true);

            DisplaySuppliersCommand = new RelayCommand(o =>
            {
                this.Source = new SuppliersVM();
            }, o => true);

            DisplayBooksCommand = new RelayCommand(o =>
            {
                this.Source = new BooksVM();
            }, o => true);

            #endregion

            BackCommand = new RelayCommand(o =>
            {
                navigatingBack = true;
                int index = backJournal.Count - 2;
                backJournal.RemoveAt(index + 1);
                this.Source = backJournal[index];
                navigatingBack = false;
            }, o => CanGoBack());
        }

        private bool CanGoBack()
        {
            if (backJournal.Count < 1)
                return false;
            else return true;
        }

        #region Inventory Mgmt
        public ICommand DisplayItemsCommand
        {
            get;
            private set;
        }

        public ICommand DisplayBooksCommand
        {
            get;
            private set;
        }

        public ICommand DisplaySuppliersCommand
        {
            get;
            private set;
        }

        #endregion

        
        public Action AboutAction
        {
            get;
            set;
        }

        public ICommand DisplayCustomModulesCommand
        {
            get;
            private set;
        }

        public ICommand DisplayAccountsCommand
        {
            get;
            private set;
        }   

        public ICommand DisplayAcademicReportCommand
        {
            get;
            private set;
        }

        public ICommand DisplayLibraryReportCommand
        {
            get;
            private set;
        }

        public ICommand DisplayStaffReportCommand
        {
            get;
            private set;
        }

        public ICommand DisplayFinanceReportCommand
        {
            get;
            private set;
        }

        public ICommand DisplayStudentsReportCommand
        {
            get;
            private set;
        }

        public ICommand DisplayBoardingReportCommand
        {
            get;
            private set;
        }
        public ICommand DisplayEventsReportCommand
        {
            get;
            private set;
        }

        public ICommand AboutCommand
        {
            get;
            private set;
        }
                
        public ICommand DisplayDisciplineDetailCommand
        {
            get;
            private set;
        }

        public ICommand DisplayEventDetailCommand
        {
            get;
            private set;
        }

        public ICommand DisplayStudentDetailCommand
        {
            get;
            private set;
        }

        public ICommand DisplayStaffDetailCommand
        {
            get;
            private set;
        }

        public ICommand DisplayStudentsCommand
        {
            get;
            private set;
        }

        public ICommand DisplayStaffCommand
        {
            get;
            private set;
        }

        public ICommand DisplayFeesCommand
        {
            get;
            private set;
        }

        public ICommand DisplayInstitutionCommand
        {
            get;
            private set;
        }
        
        public ICommand DisplayExamsCommand
        {
            get;
            private set;
        }
                
        public ICommand DisplayLibraryCommand
        {
            get;
            private set;
        }

        public ICommand DisplayGalleryCommand
        {
            get;
            private set;
        }

        public ICommand DisplaySettingsCommand
        {
            get;
            private set;
        }

        public ICommand DisplayInventoryMgmtCommand
        {
            get;
            private set;
        }

        public ICommand DisplayBoardingCommand
        {
            get;
            private set;
        }

        public ICommand DisplayPayrollCommand
        {
            get;
            private set;
        }
        
        public ICommand BackCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            
        }

    }
}
