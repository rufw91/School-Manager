using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class ModifyTimeTableVM: ViewModelBase
    {
        TimetableClassModel newTimeTable;
        ObservableCollection<ClassModel> allClasses;
        public ModifyTimeTableVM()
            : base()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            Title = "MODIFY TIMETABLE";
            NewTimeTable = new TimetableClassModel();
            AllClasses = await DataAccess.GetAllClassesAsync();
            DaysOfTheWeek= new ObservableCollection<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" }; 
            NotifyPropertyChanged("DaysOfTheWeek");
            newTimeTable.PropertyChanged += async (o, e) =>
            {
                if (e.PropertyName == "ClassID")
                {
                    if (newTimeTable.ClassID > 0)
                    {
                        newTimeTable.Entries = (await DataAccess.GetClassTimetableAsync(newTimeTable.ClassID)).Entries;
                        if (newTimeTable.Entries.Count == 0)
                        {
                            var p = await DataAccess.GetSubjectsRegistredToClassAsync(newTimeTable.ClassID);
                            foreach (var s in p)
                            {
                                newTimeTable.Entries.Add(new TimetableClassEntryModel(s));
                            }
                        }
                    }
                    else newTimeTable.Entries.Clear();
                }
            };
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o =>
            {
                bool succ = await DataAccess.SaveNewTimeTable(newTimeTable);
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Reset();
                }
                else
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);

            }, o => !IsBusy && CanSave());
        }

        private bool CanSave()
        {
            return newTimeTable.Entries.Count > 0;
        }

        public ObservableCollection<string> DaysOfTheWeek
        { get; private set;}

        public TimetableClassModel NewTimeTable
        {
            get { return this.newTimeTable; }

            set
            {
                if (value != this.newTimeTable)
                {
                    this.newTimeTable = value;
                    NotifyPropertyChanged("NewTimeTable");
                }
            }
        }

        public ObservableCollection<ClassModel> AllClasses
        {
            get { return this.allClasses; }

            private set
            {
                if (value != this.allClasses)
                {
                    this.allClasses = value;
                    NotifyPropertyChanged("AllClasses");
                }
            }
        }

        public ICommand SaveCommand
        { get; private set; }

        public override void Reset()
        {
            newTimeTable.Reset();   
        }
    }
}
