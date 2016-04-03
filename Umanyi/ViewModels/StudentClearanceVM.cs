﻿using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class StudentClearanceVM: ViewModelBase
    {
        StudentBaseModel selectedStudent;
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
            selectedStudent = new StudentBaseModel();
            selectedStudent.CheckErrors();
            selectedStudent.PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "StudentID")
                {
                    selectedStudent.CheckErrors();
                    if ( (!selectedStudent.HasErrors)&&(selectedStudent.StudentID > 0))
                        SPhoto = (await DataAccess.GetStudentAsync(selectedStudent.StudentID)).SPhoto;
                }
            };

            SelectedClassID = 0;
            IsInStudentMode = true;
            AllClasses = await DataAccess.GetAllClassesAsync();
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                bool succ;
                if (isInStudentMode)
                { 
                StudentClearancerModel st = new StudentClearancerModel();
                st.StudentID = selectedStudent.StudentID;
                st.NameOfStudent = selectedStudent.NameOfStudent;
                st.DateCleared = DateTime.Now;
                 succ = await DataAccess.SaveNewStudentClearancesAsync(new ObservableCollection<StudentClearancerModel>() { st });
            }
                else
                {
                    succ = await DataAccess.SaveNewClassClearance(selectedClassID);
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

        public StudentBaseModel SelectedStudent
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
