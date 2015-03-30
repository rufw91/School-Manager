using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    public class GenerateTimeTableVM:ViewModelBase
    {
        TimeTableModel timeTable;
        ObservableCollection<ClassModel> allClasses;
        public GenerateTimeTableVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "GENERATE TIMETABLE";
            timeTable = new TimeTableModel();           
            allClasses = await DataAccess.GetAllClassesAsync();
        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
            {
                List<SubjectModel> l;
                while (!await IsSuitableTimeTable())

                    for (int day = 0; day < 7; day++)
                    {
                        timeTable.TimeTable.Add(new KeyValuePair<int, List<KeyValuePair<int, List<SubjectBaseModel>>>>(0, new List<KeyValuePair<int, List<SubjectBaseModel>>>()));
                        for (int i = 0; i < allClasses.Count; i++)
                        {
                            timeTable.TimeTable[day].Value.Add(new KeyValuePair<int, List<SubjectBaseModel>>(allClasses[i].ClassID, new List<SubjectBaseModel>()));
                            l = new List<SubjectModel>(await DataAccess.GetSubjectsRegistredToClassAsync(allClasses[i].ClassID));
                            for (int x = 0; x < l.Count; x++)
                            {
                                int tmp = new Random().Next(l.Count);
                                timeTable.TimeTable[day].Value[i].Value.Add(l[tmp]);
                                if (await IsSuitableTimeTable())
                                    return;
                            }


                        }
                    }
            }, o => true);

        }

        private Task<bool> IsSuitableTimeTable()
        {
            return Task.Run<bool>(() =>
                {
                    bool temp = true;
                    for (int day = 0; day < 7; day++)
                    {
                        for (int i = 0; i < allClasses.Count; i++)
                        {
                            foreach (var s in timeTable.TimeTable[day].Value[i].Value)
                                if (timeTable.TimeTable[day].Value[i].Value.Count(o => o.SubjectID == s.SubjectID) > 2)
                                {
                                    temp = false;
                                    return temp;
                                }
                        }
                    }
                    return temp;
                });
        }

        public TimeTableModel TimeTable
        {
            get { return timeTable; }

            private set
            {
                if (value != timeTable)
                {
                    timeTable = value;
                    NotifyPropertyChanged("TimeTable");
                }
            }
        }

        public ICommand GenerateCommand
        { get; private set; }

        public override void Reset()
        {
        }
    }
}
