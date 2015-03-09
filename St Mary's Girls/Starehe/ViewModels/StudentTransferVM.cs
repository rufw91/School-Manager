using Helper;
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

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class StudentTransferVM:ViewModelBase
    {
        StudentSelectModel selectedStudent;
        private byte[] sPhoto;
        public StudentTransferVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "STUDENT TRANSFER";
            selectedStudent = new StudentSelectModel();
            selectedStudent.PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName == "StudentID")
                    {
                        selectedStudent.CheckErrors();
                        if ((selectedStudent.StudentID > 0) && (!selectedStudent.HasErrors))
                            SPhoto = (await DataAccess.GetStudentAsync(selectedStudent.StudentID)).SPhoto;
                    }
                };
        }

        protected override void CreateCommands()
        {
            SetTransferredCommand = new RelayCommand(async o =>
            {
                if (MessageBoxResult.Yes==MessageBox.Show("Are you sure you would like to perform this action? You can UNDO "+
                    "the operation from the Student List Window","Warning", MessageBoxButton.YesNo,MessageBoxImage.Warning))
                {
                    StudentTransferModel st = new StudentTransferModel();
                    st.StudentID = selectedStudent.StudentID;
                    st.NameOfStudent = selectedStudent.NameOfStudent;
                    st.DateTransferred = DateTime.Now;
                    bool succ = await DataAccess.SaveNewStudentTransfersAsync(new ObservableCollection<StudentTransferModel>() { st});
                    MessageBox.Show(succ ? "Succesfully saved details." : "Could not save details at this time.", succ ? "Success" : "Warning",
                    MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                    if (succ)
                        Reset();
                }
            },
            o => !selectedStudent.HasErrors);
        }

        public StudentSelectModel SelectedStudent
        {
            get { return selectedStudent; }

            set { if (selectedStudent!=value)
            {
                selectedStudent = value;
                NotifyPropertyChanged("SelectedStudent");
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

        public ICommand SetTransferredCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            SPhoto = new byte[0];
            selectedStudent.Reset();
        }
    }
}
