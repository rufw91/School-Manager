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
        CombinedClassModel selectedCombinedClass;
        DateTime currentDate;
        ObservableCollection<CombinedClassModel> allCombinedClasses;
        FeesStructureModel currentStruct;
        public ViewFeesStructureVM()
        {
            InitVars();
            CreateCommands();
        }
        public ObservableCollection<CombinedClassModel> AllCombinedClasses
        {
            get { return allCombinedClasses; }
        }

        protected async override void InitVars()
        {

            Title = "VIEW FEES STRUCTURE";
            CurrentDate = DateTime.Now;
            currentStruct = new FeesStructureModel();
            allCombinedClasses = await DataAccess.GetAllCombinedClassesAsync();
            NotifyPropertyChanged("AllCombinedClasses");
            PropertyChanged += async (o, e) =>
                {
                    if ((e.PropertyName == "CurrentDate" || e.PropertyName == "SelectedCombinedClass") && selectedCombinedClass != null && selectedCombinedClass.Entries.Count > 0)
                       CurrentStructure.Entries =  (await RefreshEntries()).Entries;
                };

        }

        protected override void CreateCommands()
        {
            
        }

        public CombinedClassModel SelectedCombinedClass
        {
            get { return selectedCombinedClass; }
            set
            {
                if (selectedCombinedClass != value)
                {
                    selectedCombinedClass = value;
                    NotifyPropertyChanged("SelectedCombinedClass");
                }     
            }
        }

        public DateTime CurrentDate
        {
            get { return currentDate; }
            set
            {
                if (currentDate != value)
                {
                    currentDate = value;
                    NotifyPropertyChanged("CurrentDate");
                }                
            }
        }

        public FeesStructureModel CurrentStructure
        {
            get { return currentStruct; }
        }

        private async Task<FeesStructureModel> RefreshEntries()
        {
            return await DataAccess.GetFeesStructureAsync(selectedCombinedClass.Entries[0].ClassID, currentDate);
        }

        public override void Reset()
        {
            
        }
    }
}
