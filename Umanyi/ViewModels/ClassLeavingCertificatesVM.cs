using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    public class ClassLeavingCertificatesVM: ViewModelBase
    {
        private CombinedClassModel selectedCombinedClass;
        private ObservableCollection<CombinedClassModel> allCombinedClasses;
        public ClassLeavingCertificatesVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "CLASS LEAVING CERTIFICATES";
            AllCerts = new ClassLeavingCertificatesModel();
            AllCombinedClasses = await DataAccess.GetAllCombinedClassesAsync();
            
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                AllCerts.Entries = await DataAccess.GetClassLeavingCerts(selectedCombinedClass.Entries);
            }, o => CanRefresh());

            GenerateCommand = new RelayCommand(o =>
            {
                var doc = DocumentHelper.GenerateDocument(AllCerts);
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(doc);
            }, o => CanGenerate());
        }

        private bool CanGenerate()
        {
            return AllCerts.Entries.Count > 0;
        }

        private bool CanRefresh()
        {
            return selectedCombinedClass != null && selectedCombinedClass.Entries.Count>0;
        }

        public CombinedClassModel SelectedCombinedClass
        {
            get { return this.selectedCombinedClass; }

            set
            {
                if (value != this.selectedCombinedClass)
                {
                    this.selectedCombinedClass = value;
                    AllCerts.Entries.Clear();
                    NotifyPropertyChanged("SelectedCombinedClass");

                }
            }
        }

        public ObservableCollection<CombinedClassModel> AllCombinedClasses
        {
            get { return this.allCombinedClasses; }

            private set
            {
                if (value != this.allCombinedClasses)
                {
                    this.allCombinedClasses = value;
                    NotifyPropertyChanged("AllCombinedClasses");
                }
            }
        }

        private ClassLeavingCertificatesModel AllCerts
        {
            get;
            set;
        }

        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }
        public ICommand GenerateCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            
        }
    }
}
