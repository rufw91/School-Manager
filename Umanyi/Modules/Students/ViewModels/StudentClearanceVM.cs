using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Modules.Students.Models;
using UmanyiSMS.Modules.Students.Controller;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.Students.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class StudentClearanceVM: ViewModelBase
    {
        StudentClearanceModel selectedStudent;

        public StudentClearanceVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "CLEAR STUDENT";
            selectedStudent = new StudentClearanceModel();
            selectedStudent.CheckErrors();
            selectedStudent.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "StudentID")
                {
                    selectedStudent.CheckErrors();
                }
            };
            
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                bool succ;
                StudentClearanceModel st = new StudentClearanceModel();
                st.StudentID = selectedStudent.StudentID;
                st.NameOfStudent = selectedStudent.NameOfStudent;
                st.DateCleared = DateTime.Now;
                 succ = await DataController.SaveNewStudentClearanceAsync(st);
            
                
                MessageBox.Show(succ ? "Succesfully saved details." : "Could not save details at this time.", succ ? "Success" : "Warning",
                    MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                if (succ)
                    Reset();
            }, o => !selectedStudent.HasErrors);
        }
        
        public StudentClearanceModel SelectedStudent
        {
            get { return selectedStudent; }

            set
            {
                if (selectedStudent != value)
                {
                    selectedStudent = value;
                    NotifyPropertyChanged("SelectedStudent");
                }
            }
        }
        
        public ICommand SaveCommand
        { get; private set; }
        public override void Reset()
        {
            selectedStudent.Reset();
        }
    }
}
