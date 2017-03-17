using Helper;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Windows.Documents;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Fees.Controller;
using UmanyiSMS.Modules.Fees.Models;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Fees.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class BalancesListVM : ViewModelBase
    {
        CombinedClassModel selectedCombinedClass;
        ObservableCollection<CombinedClassModel> allCombinedClasses;
        private FixedDocument doc;
        public BalancesListVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override async void InitVars()
        {
            Title = "BALANCES LIST";
            AllCombinedClasses = await DataController.GetAllCombinedClassesAsync();
        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                 ClassBalancesListModel s = new ClassBalancesListModel();

                foreach (var c in selectedCombinedClass.Entries)
                {
                   var t = await DataController.GetBalancesList(c);
                   foreach (var g in t.Entries)
                       s.Entries.Add(g);
                }
                s.Entries = new ObservableCollection<StudentFeesDefaultModel>(s.Entries.OrderBy(f => f.StudentID));
                s.NameOfClass = selectedCombinedClass.Description;
                foreach (var v in s.Entries)
                    s.Total += v.Balance;

                foreach (var v in s.Entries)
                    s.TotalUnpaid += (v.Balance > 0) ? v.Balance : 0;
                Document = DocumentHelper.GenerateDocument(s);
                IsBusy = false;
            }, o => selectedCombinedClass != null);
         
        }

        public ObservableCollection<CombinedClassModel> AllCombinedClasses
        {
            get { return allCombinedClasses; }
            private set
            {
                if (allCombinedClasses != value)
                {
                    allCombinedClasses = value;
                    NotifyPropertyChanged("AllCombinedClasses");
                }
            }
        }

        public override void Reset()
        {
        }

        public FixedDocument Document
        {
            get { return doc; }
            private set
            {
                if (doc != value)
                    doc = value;
                NotifyPropertyChanged("Document");
            }
        }

        public ICommand GenerateCommand
        { get; private set; }

        public CombinedClassModel SelectedCombinedClass
        {
            get { return selectedCombinedClass; }
            set
            {
                if (value != selectedCombinedClass)
                {
                    selectedCombinedClass = value;
                    NotifyPropertyChanged("SelectedCombinedClass");
                }
            }
        }
    }
}
