
using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class AcademicYearSetupVM: ViewModelBase
    {
        private AcademicYearModel newYear;
        private ObservableCollection<int> allYears;
        public AcademicYearSetupVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "ACADEMIC YEAR SETUP";
            allYears = new ObservableCollection<int>();
            for (int i = 2014; i < 2024; i++)
                allYears.Add(i);
            NewYear =  await DataAccess.GetAcademicYearAsync(DateTime.Now);
            newYear.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName=="NoOfTerms")
                    {
                        if (newYear.AllTerms.Count == newYear.NoOfTerms||newYear.NoOfTerms<0)
                            return;
                        newYear.AllTerms.Clear();
                        for(int i =0;i<newYear.NoOfTerms;i++)
                        {
                            newYear.AllTerms.Add(new TermModel() { Description = "TERM " + (i+1), StartDate = DateTime.Now.Date, EndDate = DateTime.Now.Date, TermID = i + 1 });
                        }
                    }
                };
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
             {
                 bool succ =  await DataAccess.SaveAcademicYearAsync(newYear);
                 MessageBox.Show(succ ? "Successfully saved details" : "Could not save details", succ ? "Success" : "Error", MessageBoxButton.OK,
                     succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                 
             }, o => CanSave());
        }

        private bool CanSave()
        {
            if (newYear==null||newYear.NoOfTerms < 0 || newYear.NoOfTerms > 4 || newYear.AllTerms.Count == 0 || newYear.AllTerms.Count > 4)
                return false;

            for (int i = 0; i < newYear.AllTerms.Count; i++)
            {
                if (newYear.AllTerms[i].StartDate >= newYear.AllTerms[i].EndDate)
                    return false;
                if (i < newYear.AllTerms.Count - 1)
                {
                    if (newYear.AllTerms[i].StartDate >= newYear.AllTerms[i + 1].StartDate ||
                      newYear.AllTerms[i].StartDate >= newYear.AllTerms[i + 1].EndDate ||
                      newYear.AllTerms[i].EndDate >= newYear.AllTerms[i + 1].StartDate ||
                      newYear.AllTerms[i].EndDate >= newYear.AllTerms[i + 1].EndDate)
                        return false;
                    if (!newYear.AllTerms[i].EndDate.AddDays(1).Equals(newYear.AllTerms[i+1].StartDate))
                        return false;
                }
                else
                    if (!newYear.AllTerms[i].EndDate.Equals(newYear.Year))
                        return false;

                if (i == 0)
                    if (!newYear.AllTerms[i].StartDate.Equals(newYear.Year))
                        return false;
            }

            return true;
        }

        public AcademicYearModel NewYear
        {
            get { return newYear; }
            set
            {
                if (value != newYear)
                {
                    newYear = value;
                    NotifyPropertyChanged("NewYear");
                }
            }
        }

        public ObservableCollection<int> AllYears
        {
            get { return allYears; }
        }

        public ICommand SaveCommand
        { get; private set; }
        
        public override void Reset()
        {
            newYear.Reset();
        }

        
    }
}

