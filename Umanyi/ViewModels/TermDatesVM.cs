
using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class TermDatesVM: ViewModelBase
    {
        Dictionary<int,DateTime?[]> termDates;
        public TermDatesVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "SET TERM DATES";
            termDates = new Dictionary<int,DateTime?[]>();
            for (int i = 1; i < 4; i++)
                termDates.Add(i,new DateTime?[2]);
            TermDates = await DataAccess.GetTermDatesAsync(DateTime.Now.Year);
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
             {
                 bool succ = await DataAccess.SaveTermDatesAsync(termDates, DateTime.Now.Year);
                 MessageBox.Show(succ ? "Successfully saved details" : "Could not save details", succ ? "Success" : "Error", MessageBoxButton.OK,
                     succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                 
             }, o => CanSave());
        }

        private bool CanSave()
        {
            if (termDates.Count == 0)
                return false;
            foreach (var t in termDates)
            {
                if (t.Value.Count() < 2)
                    return false;
                if (t.Value[0] == null || t.Value[1] == null)
                    return false;
                if (t.Value[0].Value >= t.Value[1].Value)
                    return false;
            }
            for (int i = 0; i < termDates.Count - 1; i++)
            {
                if (termDates.ElementAt(i).Value[1].Value >= termDates.ElementAt(i + 1).Value[0].Value)
                    return false;
                if (!termDates.ElementAt(i).Value[1].Value.AddDays(1).Equals(termDates.ElementAt(i + 1).Value[0].Value))
                    return false;
            }

            return true;
        }

        public Dictionary<int, DateTime?[]> TermDates
        {
            get { return termDates; }

            set
            {
                if (value != termDates)
                {
                    termDates = value;
                    NotifyPropertyChanged("TermDates"); 
                }
            }
        }

        public ICommand SaveCommand
        { get; private set; }

        public override void Reset()
        {
            TermDates = new Dictionary<int,DateTime?[]>();
            for (int i = 1; i < 4; i++)
                termDates.Add(i,new DateTime?[2]);
        }

        
    }
}

