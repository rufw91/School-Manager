﻿using Helper;
using Helper.Models;
using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class LeavingCertificateVM: ViewModelBase
    {
        LeavingCertificateModel leavingCert;
        StudentSelectModel selectedStudent;
        bool isCleared  = false;
        bool hasRefreshed = false;
        public LeavingCertificateVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "LEAVING CERTIFICATE";
            leavingCert = new LeavingCertificateModel();
            SelectedStudent = new StudentSelectModel();
            selectedStudent.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName != "StudentID")
                        return;
                    hasRefreshed = false;
                    isCleared=false;
                    if (selectedStudent.StudentID > 0)
                        selectedStudent.CheckErrors();
                    if (!selectedStudent.HasErrors)
                    {
                        isCleared = DataAccess.StudentIsCleared(selectedStudent.StudentID);
                        if (!isCleared)
                            selectedStudent.SetErrors("StudentID", new System.Collections.Generic.List<string>() { "Student has not been cleared." });
                    }
                };
        }

        protected override void CreateCommands()
        {
            PreviewCommand = new RelayCommand(o =>
            {
                var doc = DocumentHelper.GenerateDocument(leavingCert);
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(doc);
            }, o => CanSave());
            RefreshCommand = new RelayCommand(async o =>
             {
                 LeavingCert = await DataAccess.GetStudentLeavingCert(selectedStudent);
                 if (leavingCert.StudentID ==0)
                 {
                     var s = await DataAccess.GetStudentAsync(selectedStudent.StudentID);
                     leavingCert.StudentID = selectedStudent.StudentID;
                     leavingCert.NameOfStudent = selectedStudent.NameOfStudent;
                     leavingCert.DateOfAdmission = s.DateOfAdmission;
                     leavingCert.DateOfBirth = s.DateOfBirth;                     
                 }
                 hasRefreshed=true;
             }, o => CanRefresh());

            SaveCommand = new RelayCommand(async o =>
            {
                bool succ = await DataAccess.SaveNewLeavingCertificateAsync(leavingCert);
                MessageBox.Show(succ ? "Successfully saved details" : "Could not save details", succ ? "Success" : "Warning",
                    MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                if (succ)
                    Reset();
            }, o => CanSave());
            SaveAndPrintCommand = new RelayCommand(async o =>
            {
                bool succ = await DataAccess.SaveNewLeavingCertificateAsync(leavingCert);
                MessageBox.Show(succ ? "Successfully saved details" : "Could not save details", succ ? "Success" : "Warning",
                    MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                if (!succ)
                    return;
                    var doc = DocumentHelper.GenerateDocument(leavingCert);
                    Reset();
                    if (ShowPrintDialogAction != null)
                        ShowPrintDialogAction.Invoke(doc);


            }, o => CanSave());
        }

        private bool CanRefresh()
        {
            return !selectedStudent.HasErrors && isCleared;
        }
        private bool CanSave()
        {
            return !selectedStudent.HasErrors && isCleared && hasRefreshed;
        }
        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }
        public ICommand PreviewCommand
        {
            get;
            private set;
        }
        public ICommand RefreshCommand
        {
            get;
            private set;
        }
        public ICommand SaveCommand
        {
            get;
            private set;
        }
        public ICommand SaveAndPrintCommand
        { get; private set; }
        public LeavingCertificateModel LeavingCert
        {
            get { return leavingCert; }
            set
            {
                if (value != this.leavingCert)
                {
                    this.leavingCert = value;
                    NotifyPropertyChanged("LeavingCert");
                }
            }
        }

        public StudentSelectModel SelectedStudent
        {
            get { return selectedStudent; }
            set
            {
                if (value != this.selectedStudent)
                {
                    this.selectedStudent = value;
                    NotifyPropertyChanged("SelectedStudent");
                }
            }
        }

        public override void Reset()
        {
            selectedStudent.Reset();
            leavingCert.Reset();
            hasRefreshed = false;
            isCleared = false;
        }
    }
}
