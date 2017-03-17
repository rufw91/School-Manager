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
        private byte[] sPhoto;
        private bool isInClassMode;
        private bool isInStudentMode;
        private int selectedClassID;
        private ObservableCollection<ClassModel> allClasses;

        public StudentClearanceVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "CLEAR STUDENT";
            selectedStudent = new StudentClearanceModel();
            selectedStudent.CheckErrors();
            selectedStudent.PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "StudentID")
                {
                    selectedStudent.CheckErrors();
                    if ( (!selectedStudent.HasErrors)&&(selectedStudent.StudentID > 0))
                        SPhoto = (await DataController.GetStudentAsync(selectedStudent.StudentID)).SPhoto;
                }
            };

            SelectedClassID = 0;
            IsInStudentMode = true;
            AllClasses = await DataController.GetAllClassesAsync();
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                bool succ;
                if (isInStudentMode)
                { 
                StudentClearanceModel st = new StudentClearanceModel();
                st.StudentID = selectedStudent.StudentID;
                st.NameOfStudent = selectedStudent.NameOfStudent;
                st.DateCleared = DateTime.Now;
                 succ = await DataController.SaveNewStudentClearancesAsync(new ObservableCollection<StudentClearanceModel>() { st });
            }
                else
                {
                    succ = await DataController.SaveNewClassClearance(selectedClassID);
                }
                MessageBox.Show(succ ? "Succesfully saved details." : "Could not save details at this time.", succ ? "Success" : "Warning",
                    MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                if (succ)
                    Reset();
            }, o => isInStudentMode?!selectedStudent.HasErrors:selectedClassID>0);
        }

        public bool IsInStudentMode
        {
            get { return isInStudentMode; }

            set
            {
                if (value != isInStudentMode)
                {
                    isInStudentMode = value;
                    NotifyPropertyChanged("IsInStudentMode");
                    selectedStudent.Reset();
                }
            }
        }

        public bool IsInClassMode
        {
            get { return isInClassMode; }

            set
            {
                if (value != isInClassMode)
                {
                    isInClassMode = value;
                    NotifyPropertyChanged("IsInClassMode");
                    selectedClassID = 0;
                }
            }
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

        public ObservableCollection<ClassModel> AllClasses
        {
            get { return allClasses; }

            private set
            {
                if (value != allClasses)
                {
                    allClasses = value;
                    NotifyPropertyChanged("AllClasses");
                }
            }
        }


        public int SelectedClassID
        {
            get { return selectedClassID; }

            private set
            {
                if (value != selectedClassID)
                {
                    selectedClassID = value;
                    NotifyPropertyChanged("SelectedClassID");
                }
            }
        }

        public byte[] SPhoto
        {
            get { return sPhoto; }

            set
            {
                if (sPhoto != value)
                {
                    sPhoto = value;
                    NotifyPropertyChanged("SPhoto");
                }
            }
        }

        public ICommand SaveCommand
        { get; private set; }
        public override void Reset()
        {
            SPhoto = new byte[0];
            selectedStudent.Reset();
        }
    }
}
