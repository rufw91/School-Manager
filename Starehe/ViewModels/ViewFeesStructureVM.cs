using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class ViewFeesStructureVM: ViewModelBase
    {
        int currentClassID;
        DateTime currentDate;
        ObservableCollection<ClassModel> allClasses;
        FeesStructureModel currentStruct;
        public ViewFeesStructureVM()
        {
            InitVars();
            CreateCommands();
        }
        public ObservableCollection<ClassModel> AllClasses
        {
            get { return allClasses; }
        }

        protected async override void InitVars()
        {
            
            Title = "VIEW FEES STRUCTURE";
            CurrentClassID = 0;
            CurrentDate = DateTime.Now;
            currentStruct = new FeesStructureModel();
            allClasses = await DataAccess.GetAllClassesAsync();
        }

        protected override void CreateCommands()
        {
            
        }

        public int CurrentClassID
        {
            get { return currentClassID; }
            set
            {
                currentClassID = value;
                if (currentClassID > 0)
                    RefreshEntries();
            }
        }

        public DateTime CurrentDate
        {
            get { return currentDate; }
            set
            {
                currentDate = value;
                if (currentClassID > 0)
                    RefreshEntries();
            }
        }

        public FeesStructureModel CurrentStructure
        {
            get { return currentStruct; }
        }

        private async void RefreshEntries()
        {
            CurrentStructure.Entries = (await DataAccess.GetFeesStructureAsync(currentClassID, currentDate)).Entries;
        }

        public override void Reset()
        {
            
        }
    }
}
