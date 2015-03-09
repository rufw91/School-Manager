using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class CombinedClassListVM : ViewModelBase
    {
        CombinedClassModel selectedClass;
        ObservableCollection<CombinedClassModel> allClasses;
        private FixedDocument doc;
        public CombinedClassListVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "COMBINED CLASS LISTS";
            AllClasses = await DataAccess.GetAllCombinedClassesAsync();
        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
            {
                ClassStudentListModel s = await DataAccess.GetCombinedClassStudentListAsync(selectedClass);
                Document = DocumentHelper.GenerateDocument(s);
            }, o => selectedClass != null);
        }

        public ObservableCollection<CombinedClassModel> AllClasses
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

        public CombinedClassModel SelectedClass
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

        public override void Reset()
        {

        }
    }
}
