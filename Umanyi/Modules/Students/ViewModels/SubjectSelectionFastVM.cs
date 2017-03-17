using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Institution.Models;
using UmanyiSMS.Modules.Students.Controller;
using UmanyiSMS.Modules.Students.Models;

namespace UmanyiSMS.Modules.Students.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class SubjectSelectionFastVM:ViewModelBase
    {
        private ObservableCollection<ClassModel> allClasses;
        private int selectedClassID;
        private DataTable entries;
        private int year;
        private ObservableCollection<int> allYears;

        public SubjectSelectionFastVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "SUBJECT SELECTION (FAST)";
            allYears = new ObservableCollection<int>();
            for (int i = 2014; i < 2024; i++)
                allYears.Add(i);
            Year = DateTime.Now.Year;
             Entries = new DataTable();
            
            AllClasses = await DataController.GetAllClassesAsync();
            PropertyChanged += async (o, e) =>
                {
                   
                    if (e.PropertyName=="SelectedClassID")
                    {
                        if (selectedClassID==0)
                        return;
                        IsBusy = true;
                        var f = await DataController.GetSubjectsRegistredToClassAsync(selectedClassID);
                        DataTable rf = new DataTable();

                        rf.Columns.Add(new DataColumn("ADM NO", typeof(int)) { ReadOnly = true });
                        rf.Columns.Add(new DataColumn("NAME", typeof(string)) { ReadOnly = true });
                        foreach(var t in f)
                            rf.Columns.Add(new DataColumn(t.NameOfSubject, typeof(bool)) { Caption = t.SubjectID.ToString()});
                        
                        ObservableCollection<StudentSubjectSelectionModel> ffg = await DataController.GetClassStudentSubjectSelection(selectedClassID);
                        DataRow dtr;
                        foreach (var t in ffg)
                        {
                            dtr = rf.NewRow();
                            dtr[0] = t.StudentID;
                            dtr[1] = t.NameOfStudent;
                            for (int i = 0; i < t.Entries.Count; i++)
                                dtr[i + 2] = t.Entries[i].IsSelected;
                            rf.Rows.Add(dtr);
                        }
                        Entries = rf;
                        IsBusy = false;
                    }
                };
        }

        protected override void CreateCommands()
        {
            SelectAllCommand = new RelayCommand( o =>
                {
                    foreach (DataRow dtr in entries.Rows)
                    {
                        for (int i = 0; i < dtr.ItemArray.Length; i++)
                            if (dtr[i].GetType() == typeof(bool))
                                dtr[i] = true;
                    }
                    NotifyPropertyChanged("Entries");
                });
            SaveCommand = new RelayCommand(async o =>
                {
                    IsBusy = true;
                    ObservableCollection<StudentSubjectSelectionModel> temp = new ObservableCollection<StudentSubjectSelectionModel>();
                    StudentSubjectSelectionModel sssm;
                    StudentSubjectSelectionEntryModel sssem;
                    foreach(DataRow dtr in entries.Rows)
                    {
                        sssm = new StudentSubjectSelectionModel();
                        sssm.StudentID = int.Parse(dtr[0].ToString());
                        sssm.Year = year;
                        for (int i =2; i<dtr.ItemArray.Length;i++)
                        {
                            if (((bool)dtr.ItemArray[i]) == false)
                                continue;
                            sssem = new StudentSubjectSelectionEntryModel();
                            sssem.SubjectID = int.Parse(entries.Columns[i].Caption);                            
                            sssm.Entries.Add(sssem);
                        }
                        temp.Add(sssm);
                    }
                    var t =temp.Where(o1=>o1.Entries.Count==0);
                    for(int i =0;i<temp.Count; i++)
                    {
                        if (temp[i].Entries.Count == 0)
                            temp.RemoveAt(i);
                    }
                    bool succ=await DataController.SaveNewSubjectSelection(temp);
                    MessageBox.Show(succ ? "Successfully saved details." : "Could not save details.", succ ? "Success" : "Error", MessageBoxButton.OK,
                        succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                    IsBusy = false;
                },o=>CanSave());
        }

        private bool CanSave()
        {
            return !IsBusy;
        }

        public ObservableCollection<ClassModel> AllClasses
        {
            get { return allClasses; }
            set
            {
                if (value != this.allClasses)
                {
                    this.allClasses = value;
                    NotifyPropertyChanged("AllClasses");
                }
            }
        }

        public int Year
        {
            get { return year; }
            set
            {
                if (value != this.year)
                {
                    this.year = value;
                    NotifyPropertyChanged("Year");
                }
            }
        }

        public ObservableCollection<int> AllYears
        {
            get { return allYears; }
        }

        public DataTable Entries
        {
            get { return entries; }
            set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }

        public int SelectedClassID
        {
            get { return selectedClassID; }
            set
            {
                if (value != this.selectedClassID)
                {
                    this.selectedClassID = value;
                    NotifyPropertyChanged("SelectedClassID");
                }
            }
        }

        public ICommand SelectAllCommand
        { get; private set; }

        public ICommand SaveCommand
        { get; private set; }

        public override void Reset()
        {
        }

    }
}
