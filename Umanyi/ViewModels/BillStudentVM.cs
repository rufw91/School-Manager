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
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class BillStudentVM: ViewModelBase
    {
        bool isInStudentMode;
        bool isInClassMode;
        StudentSelectModel selectedStudent;
        CombinedClassModel selectedCombinedClass;
        FeesStructureEntryModel newEntry;
        ObservableCollection<CombinedClassModel> allCombinedClasses;
        private FeesStructureModel currentFeesStructure;
        private FeesStructureEntryModel selectedEntry;
        private decimal billTotal;
        public BillStudentVM()
        {
            InitVars();
            CreateCommands();
        }

        protected async override void InitVars()
        {
            Title = "BILL STUDENT";
            currentFeesStructure = new FeesStructureModel();
            currentFeesStructure.Entries.CollectionChanged += (o, e) =>
                {
                    BillTotal = 0;
                    foreach (var v in currentFeesStructure.Entries)
                        BillTotal += v.Amount;
                };
            newEntry = new FeesStructureEntryModel();
            IsInStudentMode = true;
            selectedCombinedClass = new CombinedClassModel();
            selectedStudent = new StudentSelectModel();
            AllCombinedClasses = await DataAccess.GetAllCombinedClassesAsync();
            selectedStudent.PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName == "StudentID")
                    {
                        currentFeesStructure.Entries.Clear();
                        SaleModel s = await DataAccess.GetThisTermInvoice(selectedStudent.StudentID);
                        foreach (var f in s.SaleItems)
                            currentFeesStructure.Entries.Add(f);
                    }
                };

            PropertyChanged += async (o, e) =>
                {
                    if ((e.PropertyName == "SelectedClass") && (isInClassMode) && (selectedCombinedClass != null) && (selectedCombinedClass.Entries.Count > 0))
                    {
                        currentFeesStructure.Entries.Clear();
                        var v = await DataAccess.GetFeesStructureAsync(selectedCombinedClass.Entries[0].ClassID, DateTime.Now);
                        foreach (var f in v.Entries)
                        {
                            currentFeesStructure.Entries.Add(f);
                        }
                    }
                };
        }

        protected override void CreateCommands()
        {
            GetFeesSturctureItemsCommand = new RelayCommand(async o =>
            {
                FeesStructureModel fs;
                if (isInStudentMode)
                    fs = await DataAccess.GetFeesStructureAsync(await DataAccess.GetClassIDFromStudentID(selectedStudent.StudentID), DateTime.Now);
                else
                    fs = fs = await DataAccess.GetFeesStructureAsync(selectedCombinedClass.Entries[0].ClassID, DateTime.Now);
                currentFeesStructure.Entries.Clear();
                foreach (var f in fs.Entries)
                {
                    currentFeesStructure.Entries.Add(f);
                }
            }, o => CanGetFeesStructure());
            AddEntryCommand = new RelayCommand(o =>
            {
                currentFeesStructure.Entries.Add(newEntry);
                NewEntry = new FeesStructureEntryModel();
            },
                o => CanAddNewEntry());

            RemoveEntryCommand = new RelayCommand(o =>
            {
                currentFeesStructure.Entries.Remove(selectedEntry);
            },
                o => CanRemoveEntry());

            SaveCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                if (isInStudentMode)
                {
                    bool succ = true;
                    if (!await DataAccess.HasInvoicedThisTerm(selectedStudent.StudentID))
                    {
                        SaleModel sm = new SaleModel();
                        sm.CustomerID = selectedStudent.StudentID;
                        sm.DateAdded = DateTime.Now;
                        sm.EmployeeID = 0;
                        sm.SaleItems = currentFeesStructure.Entries;
                        sm.RefreshTotal();

                        succ = await DataAccess.SaveNewStudentBill(sm);
                        MessageBox.Show(succ?"Successfully saved details":"Could not save details.",succ?"Success":"Error", 
                            MessageBoxButton.OK, succ?MessageBoxImage.Information:MessageBoxImage.Warning);
                        if (succ)
                            Reset();
                    }
                    else
                    {
                        bool succ2 = true;
                        SaleModel s = await DataAccess.GetThisTermInvoice(selectedStudent.StudentID);
                        s.SaleItems = currentFeesStructure.Entries;
                        succ2 = await DataAccess.UpdateStudentBill(s);
                        MessageBox.Show(succ2 ? "Successfully saved details" : "Could not save details.", succ2 ? "Success" : "Error",
                            MessageBoxButton.OK, succ2 ? MessageBoxImage.Information : MessageBoxImage.Warning);
                        if (succ2)
                            Reset();
                    }
                }
                else
                {
                    SaleModel sm;
                    bool succ = true;
                    foreach (ClassModel c in selectedCombinedClass.Entries)
                    {
                        sm = new SaleModel();
                        sm.CustomerID = c.ClassID;
                        sm.DateAdded = DateTime.Now;
                        sm.EmployeeID = 0;
                        sm.SaleItems = currentFeesStructure.Entries;
                        sm.RefreshTotal();                        
                        succ = succ&&await DataAccess.SaveNewClassBill(sm);
                    }
                    MessageBox.Show(succ ? "Successfully saved details" : "Could not save details.", succ ? "Success" : "Error",
                            MessageBoxButton.OK, succ ? MessageBoxImage.Information : MessageBoxImage.Warning);
                    if (succ)
                        Reset();
                }
                IsBusy = false;
            }, o =>CanSave());
        }

        private bool CanGetFeesStructure()
        {
            return isInStudentMode ? selectedStudent != null && !selectedStudent.HasErrors : selectedCombinedClass != null && selectedCombinedClass.Entries.Count > 0;
        }



        private bool CanAddNewEntry()
        {
            return !string.IsNullOrWhiteSpace(newEntry.Name)
                && newEntry.Amount > 0
                && (isInStudentMode?!selectedStudent.HasErrors:(selectedCombinedClass!=null)&&(selectedCombinedClass.Entries.Count>0));
        }
        private bool CanRemoveEntry()
        {
            return (selectedEntry != null);
        }

        private bool CanSave()
        {
            if (isInStudentMode)
            {
                selectedStudent.CheckErrors();
                return !selectedStudent.HasErrors;
            }
            else
            {
                return (selectedCombinedClass != null) && (selectedCombinedClass.Entries.Count > 0) && (currentFeesStructure.Entries.Count > 0);
            }
        }
        public FeesStructureEntryModel NewEntry
        {
            get { return newEntry; }
            set
            {
                if (value != newEntry)
                {
                    newEntry = value;
                    NotifyPropertyChanged("NewEntry");
                }
            }
        }
        public decimal BillTotal
        {
            get { return billTotal; }

            private set
            {
                if (value != billTotal)
                {
                    billTotal = value;
                    NotifyPropertyChanged("BillTotal");
                }
            }
        }
        public FeesStructureEntryModel SelectedEntry
        {
            get { return selectedEntry; }
            set
            {
                if (value != selectedEntry)
                {
                    selectedEntry = value;
                    NotifyPropertyChanged("SelectedEntry");
                }
            }
        }

        public StudentSelectModel SelectedStudent
        {
            get { return this.selectedStudent; }

            set
            {
                if (value != this.selectedStudent)
                {
                    this.selectedStudent = value;
                    NotifyPropertyChanged("SelectedStudent");
                }
            }
        }

        public ObservableCollection<CombinedClassModel> AllCombinedClasses
        {
            get { return this.allCombinedClasses; }

            private set
            {
                if (value != this.allCombinedClasses)
                {
                    this.allCombinedClasses = value;
                    NotifyPropertyChanged("AllCombinedClasses");
                }
            }
        }

        public CombinedClassModel SelectedCombinedClass
        {
            get { return this.selectedCombinedClass; }

            set
            {
                if (value != this.selectedCombinedClass)
                {
                    this.selectedCombinedClass = value;
                    NotifyPropertyChanged("SelectedCombinedClass");
                    
                }
            }
        }

        public bool IsInStudentMode
        {
            get { return isInStudentMode; }

            set
            {
                if (value != isInStudentMode)
                {
                    isInStudentMode = value;
                    NotifyPropertyChanged("IsInStudentMode");
                    currentFeesStructure.Reset();
                }
            }
        }

        public bool IsInClassMode
        {
            get { return isInClassMode; }

            set
            {
                if (value != isInClassMode)
                {
                    isInClassMode = value;
                    NotifyPropertyChanged("IsInClassMode");
                    currentFeesStructure.Reset();

                }
            }
        }

        public FeesStructureModel CurrentFeesStructure
        {
            get { return currentFeesStructure; }

            set
            {
                if (value != currentFeesStructure)
                {
                    currentFeesStructure = value;
                    NotifyPropertyChanged("CurrentFeesStructure");
                }
            }
        }

        public ICommand RemoveEntryCommand
        {
            get;
            private set;
        }
        public ICommand AddEntryCommand
        {
            get;
            private set;
        }
        public ICommand SaveCommand
        {
            get;
            private set;
        }

        public override void Reset()
        {
            currentFeesStructure.Entries.Clear();
            selectedStudent.Reset();
        }

        public ICommand GetFeesSturctureItemsCommand { get; private set; }
    }
}
