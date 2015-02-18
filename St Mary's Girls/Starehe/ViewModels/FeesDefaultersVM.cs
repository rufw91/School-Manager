using Helper;
using Helper.Models;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    public class FeesDefaultersVM : ViewModelBase
    {
        ClassModel selectedClass;
        ObservableCollection<ClassModel> allClasses;
        private FixedDocument doc;
        public FeesDefaultersVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override async void InitVars()
        {
            Title = "FEES DEFAULTERS";
            AllClasses = await DataAccess.GetAllClassesAsync();
        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
            {
                ClassFeesDefaultModel s = await DataAccess.GetFeesDefaulters(selectedClass);
                Document = DocumentHelper.GenerateDocument(s);
            }, o => selectedClass != null);
        }

        public ObservableCollection<ClassModel> AllClasses
        {
            get { return allClasses; }
            private set
            {
                if (allClasses != value)
                {
                    allClasses = value;
                    NotifyPropertyChanged("AllClasses");
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

        public ClassModel SelectedClass
        {
            get { return selectedClass; }
            set
            {
                if (value != selectedClass)
                {
                    selectedClass = value;
                    NotifyPropertyChanged("SelectedClass");
                }
            }
        }
    }
}
