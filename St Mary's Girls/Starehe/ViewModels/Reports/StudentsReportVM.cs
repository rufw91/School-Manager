using Helper;
using Helper.Models;
using System.Collections.ObjectModel;

namespace Starehe.ViewModels
{
    public sealed class StudentsReportVM:ViewModelBase
    {
        public StudentsReportVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            AllClasses = await DataAccess.GetAllClassesAsync();
            NotifyPropertyChanged("AllClasses");
        }

        protected override void CreateCommands()
        {
        }

        public ObservableCollection<ClassModel> AllClasses
        {
            get;
            private set;
        }

        public override void Reset()
        {
        }
    }
}
