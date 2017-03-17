using System;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Exams.ViewModels;
using UmanyiSMS.Modules.Fees.ViewModels;
using UmanyiSMS.Modules.Institution.ViewModels;
using UmanyiSMS.Modules.Library.ViewModels;
using UmanyiSMS.Modules.Purchases.ViewModels;
using UmanyiSMS.Modules.Staff.ViewModels;
using UmanyiSMS.Modules.Students.ViewModels;
using UmanyiSMS.Modules.System.Controller;

namespace UmanyiSMS.Modules.System.ViewModels
{
    public class MainWindowVM : ViewModelBase    
    {
        ViewModelBase source;
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
                }
            }
        }
        
        protected async override void InitVars()
        {
            Source = new HomePageVM();
            App.AppExamSettings.CopyFrom(await DataController.GetExamSettingsAsync());
        }

        protected override void CreateCommands()
        {
            FileHomeCommand = new RelayCommand(o => Source = new HomePageVM(), o => !(Source is HomePageVM));
            FileBackupCommand = new RelayCommand(o => { }, o => true);
            FileExitCommand = new RelayCommand(o => Application.Current.Shutdown(), o => true);
            StudentsNewStudentCommand = new RelayCommand(o => Source = new NewStudentVM() , o => true);
            StudentsModifyStudentCommand = new RelayCommand(o => Source = new ModifyStudentVM(), o => true);
            StudentsStudentListCommand = new RelayCommand(o => Source = new StudentListVM(), o => true);
            StudentsClassListNCommand = new RelayCommand(o => Source = new ClassListVM(), o => true);
            StudentsClassListCCommand = new RelayCommand(o => Source = new CombinedClassListVM(), o => true);
            StudentsSSPerStudentCommand = new RelayCommand(o => Source = new SubjectSelectionVM(), o => true);
            StudentsSSPerClassCommand = new RelayCommand(o => Source = new SubjectSelectionFastVM(), o => true);
            StudentsAssignNewClassCommand = new RelayCommand(o => Source = new AssignNewClassVM(), o => true);
            StudentsClearStudentCommand = new RelayCommand(o => Source = new StudentClearanceVM(), o => true);
            StaffNewStaffCommand = new RelayCommand(o => Source = new NewStaffVM(), o => true);
            StaffModifyStaffCommand = new RelayCommand(o => Source = new ModifyStaffVM(), o => true);
            StaffStaffListCommand = new RelayCommand(o => Source = new StaffListVM(), o => true);
            FeesNewPaymentCommand = new RelayCommand(o => Source = new NewFeesPaymentVM(), o => true);
            FeesBillStudentCommand = new RelayCommand(o => Source = new BillStudentVM(), o => true);
            FeesViewFeesStructureCommand = new RelayCommand(o => Source = new ViewFullFeesStructureVM(), o => true);
            FeesModifyFeesStructureCommand = new RelayCommand(o => Source = new SetFeesStructureVM(), o => true);
            FeesReprintReceiptCommand = new RelayCommand(o => Source = new ReprintReceiptVM(), o => true);
            FeesRemovePaymentCommand = new RelayCommand(o => Source = new RemovePaymentVM(), o => true);
            FeesRemoveBillCommand = new RelayCommand(o => Source = new RemoveBillVM(), o => true);
            FeesFeesStatementCommand = new RelayCommand(o => Source = new FeesStatementVM(), o => true);
            FeesBalancesListCommand = new RelayCommand(o => Source = new BalancesListVM(), o => true);
            FeesPaymtHGeneralCommand = new RelayCommand(o => Source = new FeesPaymentHistoryVM(), o => true);
            FeesPaymtHByVoteHeadCommand = new RelayCommand(o => Source = new PaymentsByVoteHeadVM(), o => true);
            ExamsNewExamCommand = new RelayCommand(o => Source = new NewExamVM(), o => true);
            ExamsEnterResultsPSCommand = new RelayCommand(o => Source = new EnterExamResultsVM(), o => true);
            ExamsEnterResultsPCCommand = new RelayCommand(o => Source = new EnterExamResultsBySubjectVM(), o => true);
            ExamsViewResultsCommand = new RelayCommand(o => Source = new ViewExamResultsVM(), o => true);
            ExamsReportFormPSCommand = new RelayCommand(o => Source = new StudentTranscriptVM(), o => true);
            ExamsReportFormPCCommand = new RelayCommand(o => Source = new ClassReportFormsVM(), o => true);
            ExamsMarkListNCommand = new RelayCommand(o => Source = new MarkListsVM(), o => true);
            ExamsMarkListWCommand = new RelayCommand(o => Source = new CombinedMarkListsVM(), o => true);
            ExamsSubjectPerfomanceNCommand = new RelayCommand(o => Source = new AggregateResultsVM(), o => true);
            ExamsSubjectPerfomanceCCommand = new RelayCommand(o => Source = new CombinedAggregateResultsVM(), o => true);
            ExamsRemoveExamCommand = new RelayCommand(o => Source = new RemoveExamVM(), o => true);
            PurchasesNewPurchaseCommand = new RelayCommand(o => Source = new ReceiveItemsVM(), o => true);
            PurchasesPHistoryCommand = new RelayCommand(o => Source = new ItemReceiptHistoryVM(), o => true);
            PurchasesNewItemCommand = new RelayCommand(o => Source = new NewItemVM(), o => true);
            PurchasesNewItemCategoryCommand = new RelayCommand(o => Source = new NewItemCategoryVM(), o => true);
            PurchasesModifyItemCommand = new RelayCommand(o => Source = new ModifyItemVM(), o => true);
            PurchasesRemoveItemCommand = new RelayCommand(o => { }/* Source = new RemoveItemVM()*/, o => false);
            PurchasesNewSupplierCommand = new RelayCommand(o => Source = new NewSupplierVM(), o => true);
            PurchasesModifySupplierCommand = new RelayCommand(o => Source = new ModifySupplierVM(), o => true);
            PurchasesSupplierListCommand = new RelayCommand(o => Source = new SupplierListVM(), o => true);
            PurchasesPaymentToSupplierCommand = new RelayCommand(o => Source = new PaymentToSupplierVM(), o => true);
            PurchasesSupplierStatementCommand = new RelayCommand(o => Source = new SupplierStatementVM(), o => true);
            PurchasesSPaymtHistoryCommand = new RelayCommand(o => Source = new PaymentToSupplierHistoryVM(), o => true);
            PurchasesReprintPVCommand = new RelayCommand(o => Source = new ReprintPaymentVoucherVM(), o => true);
            PurchasesRemoveSupplierCommand = new RelayCommand(o => Source = new RemoveSupplierVM(), o => false);
            LibraryIssueBookCommand = new RelayCommand(o => Source = new IssueBookVM(), o => true);
            LibraryReturnBookCommand = new RelayCommand(o => Source = new BookReturnVM(), o => true);
            LibraryNewBookCommand = new RelayCommand(o => Source = new NewBookVM(), o => true);
            LibraryModifyBookCommand = new RelayCommand(o => Source = new ModifyBookVM(), o => true);
            LibraryBookListCommand = new RelayCommand(o => Source = new ViewBooksVM(), o => true);
            LibraryUnreturnedBooksCommand = new RelayCommand(o => Source = new UnreturnedBooksVM(), o => true);
            
            SettingsInstInfoCommand = new RelayCommand(o => Source = new InstitutionSetupVM(), o => true);
            SettingsACYSCommand = new RelayCommand(o => Source = new AcademicYearSetupVM(), o => true);
            SettingsCSCommand = new RelayCommand(o => Source = new ClassesSetupVM(), o => true);
            SettingsISSCommand = new RelayCommand(o => Source = new InstitutionSubjectsSetupVM(), o => true);
            SettingsGSCommand = new RelayCommand(o => Source = new ExamSetupVM(), o => true);
            SettingsATCommand = new RelayCommand(o => { }, o => false);
            SettingsAdvancedCommand = new RelayCommand(o => Source = new AdvancedSettingsVM(), o => true);
            HelpGetHelpCommand = new RelayCommand(o => { if (HelpGetHelpAction != null) HelpGetHelpAction.Invoke(); }, o => true);
            HelpAboutCommand = new RelayCommand(o => { if (HelpAboutAction != null) HelpAboutAction.Invoke(); }, o => true);
        }

        #region Command Stuff

        public ICommand FileHomeCommand
        {
            get;
            private set;
        }

        public ICommand FileBackupCommand
        {
            get;
            private set;
        }

        public ICommand FileExitCommand
        {
            get;
            private set;
        }

        #region Students
        public ICommand StudentsNewStudentCommand
        {
            get;
            private set;
        }

        public ICommand StudentsModifyStudentCommand
        {
            get;
            private set;
        }

        public ICommand StudentsStudentListCommand
        {
            get;
            private set;
        }

        public ICommand StudentsClassListNCommand
        {
            get;
            private set;
        }

        public ICommand StudentsClassListCCommand
        {
            get;
            private set;
        }

        public ICommand StudentsSSPerStudentCommand
        {
            get;
            private set;
        }

        public ICommand StudentsSSPerClassCommand
        {
            get;
            private set;
        }

        public ICommand StudentsAssignNewClassCommand
        {
            get;
            private set;
        }

        public ICommand StudentsClearStudentCommand
        {
            get;
            private set;
        }
        #endregion

        #region Staff
        public ICommand StaffNewStaffCommand
        {
            get;
            private set;
        }

        public ICommand StaffModifyStaffCommand
        {
            get;
            private set;
        }

        public ICommand StaffStaffListCommand
        {
            get;
            private set;
        }
        #endregion

        #region Fees
        public ICommand FeesNewPaymentCommand
        {
            get;
            private set;
        }

        public ICommand FeesBillStudentCommand
        {
            get;
            private set;
        }

        public ICommand FeesViewFeesStructureCommand
        {
            get;
            private set;
        }

        public ICommand FeesModifyFeesStructureCommand
        {
            get;
            private set;
        }

        public ICommand FeesReprintReceiptCommand
        {
            get;
            private set;
        }

        public ICommand FeesRemovePaymentCommand
        {
            get;
            private set;
        }

        public ICommand FeesRemoveBillCommand
        {
            get;
            private set;
        }

        public ICommand FeesFeesStatementCommand
        {
            get;
            private set;
        }

        public ICommand FeesBalancesListCommand
        {
            get;
            private set;
        }

        public ICommand FeesPaymtHGeneralCommand
        {
            get;
            private set;
        }

        public ICommand FeesPaymtHByVoteHeadCommand
        {
            get;
            private set;
        }
        #endregion

        #region Exams
        public ICommand ExamsNewExamCommand
        {
            get;
            private set;
        }

        public ICommand ExamsEnterResultsPSCommand
        {
            get;
            private set;
        }

        public ICommand ExamsEnterResultsPCCommand
        {
            get;
            private set;
        }

        public ICommand ExamsViewResultsCommand
        {
            get;
            private set;
        }

        public ICommand ExamsReportFormPSCommand
        {
            get;
            private set;
        }

        public ICommand ExamsReportFormPCCommand
        {
            get;
            private set;
        }

        public ICommand ExamsMarkListNCommand
        {
            get;
            private set;
        }

        public ICommand ExamsMarkListWCommand
        {
            get;
            private set;
        }

        public ICommand ExamsSubjectPerfomanceNCommand
        {
            get;
            private set;
        }

        public ICommand ExamsSubjectPerfomanceCCommand
        {
            get;
            private set;
        }

        public ICommand ExamsRemoveExamCommand
        {
            get;
            private set;
        }
        #endregion

        #region Purchases
        public ICommand PurchasesNewPurchaseCommand
        {
            get;
            private set;
        }

        public ICommand PurchasesPHistoryCommand
        {
            get;
            private set;
        }

        public ICommand PurchasesNewItemCommand
        {
            get;
            private set;
        }

        public ICommand PurchasesNewItemCategoryCommand
        {
            get;
            private set;
        }

        public ICommand PurchasesModifyItemCommand
        {
            get;
            private set;
        }

        public ICommand PurchasesRemoveItemCommand
        {
            get;
            private set;
        }

        public ICommand PurchasesNewSupplierCommand
        {
            get;
            private set;
        }

        public ICommand PurchasesModifySupplierCommand
        {
            get;
            private set;
        }

        public ICommand PurchasesSupplierListCommand
        {
            get;
            private set;
        }

        public ICommand PurchasesPaymentToSupplierCommand
        {
            get;
            private set;
        }

        public ICommand PurchasesSupplierStatementCommand
        {
            get;
            private set;
        }

        public ICommand PurchasesSPaymtHistoryCommand
        {
            get;
            private set;
        }

        public ICommand PurchasesReprintPVCommand
        {
            get;
            private set;
        }

        public ICommand PurchasesRemoveSupplierCommand
        {
            get;
            private set;
        }
        #endregion

        #region Library
        public ICommand LibraryIssueBookCommand
        {
            get;
            private set;
        }

        public ICommand LibraryReturnBookCommand
        {
            get;
            private set;
        }

        public ICommand LibraryNewBookCommand
        {
            get;
            private set;
        }

        public ICommand LibraryModifyBookCommand
        {
            get;
            private set;
        }

        public ICommand LibraryBookListCommand
        {
            get;
            private set;
        }

        public ICommand LibraryUnreturnedBooksCommand
        {
            get;
            private set;
        }
        #endregion
        
        #region Settings
        public ICommand SettingsInstInfoCommand
        {
            get;
            private set;
        }

        public ICommand SettingsACYSCommand
        {
            get;
            private set;
        }

        public ICommand SettingsCSCommand
        {
            get;
            private set;
        }

        public ICommand SettingsISSCommand
        {
            get;
            private set;
        }
        
        public ICommand SettingsGSCommand
        {
            get;
            private set;
        }

        public ICommand SettingsATCommand
        {
            get;
            private set;
        }

        public ICommand SettingsAdvancedCommand
        {
            get;
            private set;
        }
        #endregion

        #region  Help
        public ICommand HelpGetHelpCommand
        {
            get;
            private set;
        }

        public ICommand HelpAboutCommand
        {
            get;
            private set;
        }
        #endregion
        #endregion

        public Action HelpAboutAction
        { get; set; }

        public Action HelpGetHelpAction
        { get; set; }


        public override void Reset()
        {
            
        }

    }
}
