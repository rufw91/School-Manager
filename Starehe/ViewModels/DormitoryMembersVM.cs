using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class DormitoryMembersVM: ViewModelBase
    {        
        ObservableCollection<DormitoryMemberModel> entries;
        int selectedDormitoryID;
        public DormitoryMembersVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "DORMITORY MEMBERS";
            Entries = new ObservableCollection<DormitoryMemberModel>();
            PropertyChanged += async (o, e) =>
                {
                    if ((e.PropertyName == "SelectedDormitoryID") &&
                       (selectedDormitoryID > 0))
                    {
                        entries.Clear();
                        Entries = await DataAccess.GetDormitoryMembers(selectedDormitoryID);
                    }
                };
            AllDorms = await DataAccess.GetAllDormsAsync();
            NotifyPropertyChanged("AllDorms");
        }

        protected override void CreateCommands()
        {
        }

        public int SelectedDormitoryID
        {
            get { return this.selectedDormitoryID; }

            private set
            {
                if (value != this.selectedDormitoryID)
                {
                    this.selectedDormitoryID = value;
                    NotifyPropertyChanged("SelectedDormitoryID");
                }
            }
        }

        public ObservableCollection<DormitoryMemberModel> Entries
        {
            get { return this.entries; }

            private set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }

        public ObservableCollection<DormModel> AllDorms
        {
            get;
            private set;
        }

        public override void Reset()
        {
            SelectedDormitoryID = 0;
            Entries = new ObservableCollection<DormitoryMemberModel>();
        }
    }
}
