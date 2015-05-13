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

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class ModifyTimeTableFullVM:ViewModelBase
    {
        TimeTableModel timeTable;
        public ModifyTimeTableFullVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "MODIFY TIMETABLE (FULL)";
            IsBusy = true;
            CurrentTimeTable = await DataAccess.GetCurrentTimeTableFull();
            IsBusy = false;
        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(o =>
            {
               
            }, o => true);

            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                bool succ = await DataAccess.SaveNewTimeTable(timeTable);
                IsBusy = false;
                MessageBox.Show(succ?"Successfully saved time table.":"Could not save time table.", succ?"Success":"Error", MessageBoxButton.OK,
                    succ?MessageBoxImage.Information:MessageBoxImage.Warning);
                
            }, o => !IsBusy);

        }

        private Task<bool> IsSuitableTimeTable()
        {
            return Task.Run<bool>(() =>
                {
                    bool temp = true;
                    return temp;
                });
        }

        public TimeTableModel CurrentTimeTable
        {
            get { return timeTable; }
            private set
            {
                if (value != timeTable)
                {
                    timeTable = value;
                    NotifyPropertyChanged("CurrentTimeTable");
                }
            }
        }

        public ICommand GenerateCommand
        { get; private set; }

        public ICommand SaveCommand
        { get; private set; }

        public override void Reset()
        {
        }
    }
}
