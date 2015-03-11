using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class DisciplineTrackerHistoryVM : ViewModelBase
    {
        StudentSelectModel selectedStudent;
        private DateTime? start;
        private DateTime? end;
        ObservableCollection<DisciplineModel> entries;
        public DisciplineTrackerHistoryVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "DISCIPLINE HISTORY";
            SelectedStudent = new StudentSelectModel();
            selectedStudent.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "StudentID")
                    selectedStudent.CheckErrors();
                entries.Clear();
            };
            entries = new ObservableCollection<DisciplineModel>();
        }

        protected override void CreateCommands()
        {
            RefreshCommand = new RelayCommand(async o =>
            {
                Entries = await DataAccess.GetStudentDiscipline(selectedStudent, start, end);
            }, o => CanRefresh());
        }

        private bool CanRefresh()
        {
            if (selectedStudent.HasErrors)
            {
                if (start != null && end != null)
                    return true;
            }
            else            
                return true;            
            return false;
        }

        public override void Reset()
        {

        }

        public StudentSelectModel SelectedStudent
        {
            get { return selectedStudent; }
            set
            {
                if (value != selectedStudent)
                    selectedStudent = value;
                NotifyPropertyChanged("SelectedStudent");
            }
        }

        public ObservableCollection<DisciplineModel> Entries
        {
            get { return entries; }
            private set
            {
                if (value != entries)
                    entries = value;
                NotifyPropertyChanged("Entries");
            }
        }

        public DateTime? Start
        {
            get { return start; }
            set
            {
                if (value != start)
                    start = value;
                NotifyPropertyChanged("Start");
            }
        }

        public DateTime? End
        {
            get { return end; }
            set
            {
                if (value != end)
                    end = value;
                NotifyPropertyChanged("End");
            }
        }

        public ICommand RefreshCommand
        {
            get;
            private set;
        }
    }

}