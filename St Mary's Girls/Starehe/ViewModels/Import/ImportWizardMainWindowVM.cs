using Helper;
using Helper.Models;
using OpenXmlPackaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Principal")]
    public class ImportWizardMainWindowVM : ViewModelBase
    {
        ViewModelBase source;
        string pathToFile;
        string dimension;
        DataTable data;
        ObservableCollection<string> allColumns;
        string studentIDColumn;
        string firstNameColumn;
        string middleNameColumn;
        string lastNameColumn;
        string classColumn;
        string phoneNoColumn;
        string balanceBFColumn;
        private int testProgressStudentID;
        private int testProgressFirstName;
        private int testProgressMiddleName;
        private int testProgressLastName;
        private int testProgressClass;
        private int testProgressPhoneNo;
        private int testProgressBalanceBF;
        private bool hasFinished;
        private int progress;
        private string progressText;
        private string informationText;

        public ImportWizardMainWindowVM()
        {
            InitVars();
            CreateCommands();
        }

        public Dictionary<int, List<string>> Errors
        { get; private set; }

        public ViewModelBase Source
        {
            get { return this.source; }

            set
            {
                if (value != this.source)
                {
                    this.source = value;
                    NotifyPropertyChanged("Source");
                }
            }
        }

        protected async override void InitVars()
        {
            IsBusy = true;
            HasFinished = false;
            InformationText = "Thing are all setup. Click Start to begin the import process.";
            Progress = 0;
            AllColumns = new ObservableCollection<string>();
            Source = new ImportWizardPage1VM();
            AllClasses = new List<ClassModel>(await DataAccess.GetAllClassesAsync());
           
            
            Errors = new Dictionary<int, List<string>>();
            IsBusy = false;
        }

        public string InformationText
        {
            get { return informationText; }
            private set
            {
                if (value != this.informationText)
                {
                    this.informationText = value;
                    NotifyPropertyChanged("InformationText");
                }
            }
        }

        protected override void CreateCommands()
        {
            ImportCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                await Import();
                IsBusy = false;
            }, o => !IsBusy);
            BrowseCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                PathToFile = FileHelper.BrowseExcelFileAsString();
                if (!string.IsNullOrWhiteSpace(pathToFile))
                await Task.Run(() =>
                {
                    Dimension = SpreadsheetDocument.GetDimension(pathToFile);
                });
                IsBusy = false;
            }, o => !IsBusy);
            CancelCommand = new RelayCommand(o =>
            {
                if (CloseAction != null)
                    CloseAction.Invoke();
            }, o => !IsBusy);
            NextCommand = new RelayCommand(async o =>
            {
                IsBusy = true;
                await GoNext();
                IsBusy = false;
            }, o => !IsBusy && CanGoNext());
        }

        private Task Import()
        {
            return Task.Run(async () =>
            {
                ProgressText = "Initializing...";
                //Check Classes
                List<string> newClasses = new List<string>();

                ProgressText = "Checking for New Classes.";
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dtr = data.Rows[i];
                    Progress = Convert.ToInt32(((double)(i * 100) / (double)data.Rows.Count));
                    if (Errors.ContainsKey(i))
                        continue;

                    string newClass = dtr[classColumn].ToString();
                    if ((!newClasses.Any(s => s.ToUpper() == newClass.ToUpper())) && (!AllClasses.Any(v => v.NameOfClass.ToUpper() == newClass.ToUpper())))
                        newClasses.Add(newClass.ToUpper());
                }


                //Save Category
                ProgressText = "Saving new Classes.";
                for (int i = 0; i < newClasses.Count; i++)
                {
                    Progress = Convert.ToInt32(((double)(i * 100) / (double)newClasses.Count));
                    ClassesSetupModel classesSetup = new ClassesSetupModel();
                    ObservableCollection<ClassModel> allClasses = await DataAccess.GetAllClassesAsync();
                    foreach (ClassModel c in allClasses)
                        classesSetup.Entries.Add(new ClassesSetupEntryModel(c));

                    ClassModel cm = new ClassModel(0, newClasses[i]);
                    classesSetup.Entries.Add(new ClassesSetupEntryModel(cm));

                    await DataAccess.SaveNewClassSetupAsync(classesSetup);
                }

                Progress = 0;
                ProgressText = "Refreshing Classes.";
                AllClasses = new List<ClassModel>( DataAccess.GetAllClassesAsync().Result);                
                Progress = 100;

                //Save Students                   
                StudentModel student = new StudentModel();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dtr = data.Rows[i];
                    Progress = Convert.ToInt32(((double)(i * 100) / (double)data.Rows.Count));
                    if (Errors.ContainsKey(i))
                        continue;
                    student.StudentID = int.Parse(dtr[studentIDColumn].ToString());
                    student.FirstName = dtr[firstNameColumn].ToString();
                    student.MiddleName = dtr[middleNameColumn].ToString();
                    student.LastName = dtr[lastNameColumn].ToString();
                    ProgressText = "Saving Student:  " + student.NameOfStudent;
                    student.Address = "X";
                    student.BedNo = "X";
                    student.ClassID = AllClasses.First(c => c.NameOfClass.ToUpper() == dtr[classColumn].ToString().ToUpper()).ClassID;
                    student.DateOfAdmission = DateTime.Now;
                    student.DateOfBirth = new DateTime(2000, 1, 1);
                    student.Email = "test@example.com";
                    student.GuardianPhoneNo = dtr[phoneNoColumn].ToString();
                    student.NameOfGuardian = "X";
                    student.PostalCode = "X";
                    student.PrevBalance = decimal.Parse(dtr[balanceBFColumn].ToString());
                   
                    await DataAccess.SaveNewStudentAsync(student,false);
                }

                ProgressText = "Successfully saved Students.";
                    HasFinished =true;
                    InformationText = "Successfully completed Import Process.";
            });
        }

        private List<ClassModel> AllClasses
        { get; set; }
        
        private async Task GoNext()
        {
            if (source is ImportWizardPage1VM)
            {
                this.Source = new ImportWizardPage2VM();
                IsBusy = true;
                Data = await GetData();
                GetAllColumns();
                Data = await CleanData(Data);
                IsBusy = false;
                return;
            }
            if (source is ImportWizardPage2VM)
            {
                this.Source = new ImportWizardPage3VM();
                return;
            }
            if (source is ImportWizardPage3VM)
            {
                if (Errors.Count > 0)
                    if (MessageBoxResult.Yes == MessageBox.Show("Some errors occured while tring to parse the data.\r\n"
                        + "The wizard will automatically skip items with errors. Do you want to see the errors? ", "Error(s)", MessageBoxButton.YesNo, MessageBoxImage.Warning))
                    {
                        if (ShowErrorsAction != null)
                            ShowErrorsAction.Invoke(Errors);
                    }
                this.Source = new ImportWizardPage4VM();
                return;
            }
        }

        private void GetAllColumns()
        {
            foreach (DataColumn dt in data.Columns)
                AllColumns.Add(dt.ColumnName);
        }

        private Task<DataTable> GetData()
        {
            return Task.Run<DataTable>(() =>
            {
                DataTable dt = new DataTable();
                try
                {
                    using (var doc = SpreadsheetDocument.Open(pathToFile))
                    {
                        Worksheet x = doc.Worksheets[0];
                        dt = x.ExportDataTable();
                    }
                }
                catch { }
                return dt;
            });
        }

        private Task<DataTable> CleanData(DataTable rawData)
        {
            return Task.Run<DataTable>(() =>
            {
                DataTable dt = rawData;
                foreach (DataRow dtr in dt.Rows)
                {
                    for (int i =0 ; i<dt.Columns.Count;i++)
                    {
                        dtr[i]=dtr[i].ToString().Replace("'", "");
                    }
                }
                return dt;
            });
        }

        private bool CanGoNext()
        {
            if (source is ImportWizardPage1VM)
                return FileHelper.Exists(pathToFile) && (string.IsNullOrEmpty(dimension) ? true : Utilities.ValidateRangeReference(dimension));

            if (source is ImportWizardPage2VM)
                return true;

            if (source is ImportWizardPage3VM)
            {
                bool stringsAreNull = string.IsNullOrWhiteSpace(phoneNoColumn) ||
                    string.IsNullOrWhiteSpace(firstNameColumn) || string.IsNullOrWhiteSpace(classColumn) ||
                    string.IsNullOrWhiteSpace(lastNameColumn) || string.IsNullOrWhiteSpace(middleNameColumn) ||
                    string.IsNullOrWhiteSpace(studentIDColumn);
                bool prog100 = testProgressClass == 100 && testProgressFirstName == 100 && testProgressMiddleName == 100
                    && testProgressLastName == 100 && testProgressPhoneNo == 100 && testProgressPhoneNo == 100;

                //return !stringsAreNull && prog100;
                return true;
            }
            return false;
        }

        public override void Reset()
        {
        }

        public Action CloseAction
        { get; set; }

        public Action<Dictionary<int, List<string>>> ShowErrorsAction
        { get; set; }

        public string PathToFile
        {
            get { return pathToFile; }
            private set
            {
                if (value != this.pathToFile)
                {
                    this.pathToFile = value;
                    NotifyPropertyChanged("PathToFile");
                }
            }
        }

        public string ProgressText
        {
            get { return progressText; }
            private set
            {
                if (value != this.progressText)
                {
                    this.progressText = value;
                    NotifyPropertyChanged("ProgressText");
                }
            }
        }

        public DataTable Data
        {
            get { return data; }
            private set
            {
                if (value != this.data)
                {
                    this.data = value;
                    NotifyPropertyChanged("Data");
                }
            }
        }

        public string Dimension
        {
            get { return dimension; }
            set
            {
                if (value != this.dimension)
                {
                    this.dimension = value;
                    NotifyPropertyChanged("Dimension");
                }
            }
        }

        public ObservableCollection<string> AllColumns
        {
            get { return allColumns; }
            private set
            {
                if (value != this.allColumns)
                {
                    this.allColumns = value;
                    NotifyPropertyChanged("AllColumns");
                }
            }
        }

        public bool HasFinished
        {
            get { return hasFinished; }
            private set
            {
                if (value != this.hasFinished)
                {
                    this.hasFinished = value;
                    NotifyPropertyChanged("HasFinished");
                }
            }
        }

        public int Progress
        {
            get { return progress; }
            private set
            {
                if (value != this.progress)
                {
                    this.progress = value;
                    NotifyPropertyChanged("Progress");
                }
            }
        }

        public string StudentIDColumn
        {
            get { return studentIDColumn; }
            set
            {
                if (value != this.studentIDColumn)
                {
                    this.studentIDColumn = value;
                    NotifyPropertyChanged("StudentIDColumn");
                    TestAll();
                }
            }
        }

        public string FirstNameColumn
        {
            get { return firstNameColumn; }
            set
            {
                if (value != this.firstNameColumn)
                {
                    this.firstNameColumn = value;
                    NotifyPropertyChanged("FirstNameColumn");
                    TestAll();
                }
            }
        }

        public string MiddleNameColumn
        {
            get { return middleNameColumn; }
            set
            {
                if (value != this.middleNameColumn)
                {
                    this.middleNameColumn = value;
                    NotifyPropertyChanged("MiddleNameColumn");
                    TestAll();
                }
            }
        }

        public string LastNameColumn
        {
            get { return lastNameColumn; }
            set
            {
                if (value != this.lastNameColumn)
                {
                    this.lastNameColumn = value;
                    NotifyPropertyChanged("LastNameColumn");
                    TestAll();
                }
            }
        }

        public string ClassColumn
        {
            get { return classColumn; }
            set
            {
                if (value != this.classColumn)
                {
                    this.classColumn = value;
                    NotifyPropertyChanged("ClassColumn");
                    TestAll(); 
                }
            }
        }

        public string PhoneNoColumn
        {
            get { return phoneNoColumn; }
            set
            {
                if (value != this.phoneNoColumn)
                {
                    this.phoneNoColumn = value;
                    NotifyPropertyChanged("PhoneNoColumn");
                    TestAll();
                }
            }
        }

        public string BalanceBFColumn
        {
            get { return balanceBFColumn; }
            set
            {
                if (value != this.balanceBFColumn)
                {
                    this.balanceBFColumn = value;
                    NotifyPropertyChanged("BalanceBFColumn");
                    TestAll();
                }
            }
        }

        private void AddError(int i, string error)
        {
            if (Errors.ContainsKey(i))
                Errors[i].Add(error);
            else
                Errors.Add(i, new List<string>() { error });
        }

        private async void TestAll()
        {
            Errors.Clear();
            await Task.WhenAll<bool>(TestStudentID(), TestFirstName(), TestMiddleName(), TestLastName(),
                TestClass(), TestPhoneNo(), TestBalanceBF());
        }

        private async Task<bool> TestStudentID()
        {
            TestProgressStudentID = 0;
            if (string.IsNullOrWhiteSpace(studentIDColumn))
            {
                TestProgressStudentID = 100;
                return false;
            }
            
            return await Task.Run<bool>(() =>
            {
                bool succ = true;
                bool isOk;
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    TestProgressStudentID = Convert.ToInt32(((double)(i * 100) / (double)data.Rows.Count));
                    int x;
                    isOk = int.TryParse(data.Rows[i][studentIDColumn] == null ? null : data.Rows[i][studentIDColumn].ToString(), out x)
                        && x >0;
                    succ = succ && isOk;
                    if (!isOk)
                        AddError(i, "Student ID value [" + data.Rows[i][studentIDColumn] + "] is invalid.");
                }
                return succ;
            });
        }

        private async Task<bool> TestFirstName()
        {
            TestProgressFirstName = 0;
            if (string.IsNullOrWhiteSpace(firstNameColumn))
            {
                TestProgressFirstName = 100;
                return false;
            }
            return await Task.Run<bool>(() =>
            {
                bool succ = true;
                bool isOk;
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    TestProgressFirstName = Convert.ToInt32(((double)(i * 100) / (double)data.Rows.Count));
                    isOk = !string.IsNullOrWhiteSpace(data.Rows[i][firstNameColumn] == null ? null : data.Rows[i][firstNameColumn].ToString());
                    succ = succ && isOk;
                    if (!isOk)
                        AddError(i, "First Name value [" + data.Rows[i][firstNameColumn] + "] is invalid.");
                }
                return succ;
            });
        }

        private async Task<bool> TestMiddleName()
        {
            TestProgressMiddleName = 0;
            if (string.IsNullOrWhiteSpace(middleNameColumn))
            {
                TestProgressMiddleName = 100;
                return false;
            }
            return await Task.Run<bool>(() =>
            {
                bool succ = true;
                bool isOk;
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    TestProgressMiddleName = Convert.ToInt32(((double)(i * 100) / (double)data.Rows.Count));
                    isOk = !string.IsNullOrWhiteSpace(data.Rows[i][middleNameColumn] == null ? null : data.Rows[i][middleNameColumn].ToString());
                    succ = succ && isOk;
                    if (!isOk)
                        AddError(i, "Middle name value [" + data.Rows[i][middleNameColumn] + "] is invalid.");
                }
                return succ;
            });
        }

        private async Task<bool> TestLastName()
        {
            TestProgressLastName = 0;
            if (string.IsNullOrWhiteSpace(lastNameColumn))
            {
                TestProgressLastName = 100;
                return false;
            }
            return await Task.Run<bool>(() =>
            {
                bool succ = true;
                bool isOk;
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    TestProgressLastName = Convert.ToInt32(((double)(i * 100) / (double)data.Rows.Count));
                    isOk = !string.IsNullOrWhiteSpace(data.Rows[i][lastNameColumn] == null ? null : data.Rows[i][lastNameColumn].ToString());
                    succ = succ && isOk;
                    if (!isOk)
                        AddError(i, "Last Name value [" + data.Rows[i][lastNameColumn] + "] is invalid.");
                }
                return succ;
            });
        }

        private async Task<bool> TestClass()
        {
            TestProgressClass = 0;
            if (string.IsNullOrWhiteSpace(classColumn))
            {
                TestProgressClass = 100;
                return false;
            }
            return await Task.Run<bool>(() =>
            {
                bool succ = true;
                bool isOk;
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    TestProgressClass = Convert.ToInt32(((double)(i * 100) / (double)data.Rows.Count));
                    isOk = !string.IsNullOrWhiteSpace(data.Rows[i][classColumn] == null ? null : data.Rows[i][classColumn].ToString());
                    succ = succ && isOk;
                    if (!isOk)
                        AddError(i, "Class value [" + data.Rows[i][classColumn] + "] is invalid.");
                }
                return succ;
            });
        }

        private async Task<bool> TestPhoneNo()
        {
            TestProgressPhoneNo = 0;
            if (string.IsNullOrWhiteSpace(phoneNoColumn))
            {
                TestProgressPhoneNo = 100;
                return false;
            }
            return await Task.Run<bool>(() =>
            {
                bool succ = true;
                bool isOk;
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    TestProgressPhoneNo = Convert.ToInt32(((double)(i * 100) / (double)data.Rows.Count));
                    isOk = !string.IsNullOrWhiteSpace(data.Rows[i][phoneNoColumn] == null ? null : data.Rows[i][phoneNoColumn].ToString());
                    succ = succ && isOk;
                    if (!isOk)
                        AddError(i, "Description value [" + data.Rows[i][phoneNoColumn] + "] is invalid.");
                }
                return succ;
            });
        }

        private async Task<bool> TestBalanceBF()
        {
            TestProgressBalanceBF = 0;
            if (string.IsNullOrWhiteSpace(balanceBFColumn))
            {
                TestProgressBalanceBF = 100;
                return false;
            }
            return await Task.Run<bool>(() =>
            {
                bool succ = true;
                bool isOk;
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    TestProgressBalanceBF = Convert.ToInt32(((double)(i * 100) / (double)data.Rows.Count));
                    Decimal test;
                    isOk = decimal.TryParse(data.Rows[i][balanceBFColumn].ToString(),out test);
                    succ = succ && isOk;
                    if (!isOk)
                        AddError(i, "Description value [" + data.Rows[i][balanceBFColumn] + "] is invalid.");
                }
                return succ;
            });
        }

        public int TestProgressPhoneNo
        {
            get { return testProgressPhoneNo; }
            set
            {
                if (value != this.testProgressPhoneNo)
                {
                    this.testProgressPhoneNo = value;
                    NotifyPropertyChanged("TestProgressPhoneNo");
                }
            }
        }

        public int TestProgressBalanceBF
        {
            get { return testProgressBalanceBF; }
            set
            {
                if (value != this.testProgressBalanceBF)
                {
                    this.testProgressBalanceBF = value;
                    NotifyPropertyChanged("TestProgressBalanceBF");
                }
            }
        }

        public int TestProgressClass
        {
            get { return testProgressClass; }
            set
            {
                if (value != this.testProgressClass)
                {
                    this.testProgressClass = value;
                    NotifyPropertyChanged("TestProgressClass");
                }
            }
        }

        public int TestProgressFirstName
        {
            get { return testProgressFirstName; }
            set
            {
                if (value != this.testProgressFirstName)
                {
                    this.testProgressFirstName = value;
                    NotifyPropertyChanged("TestProgressFirstName");
                }
            }
        }

        public int TestProgressLastName
        {
            get { return testProgressLastName; }
            set
            {
                if (value != this.testProgressLastName)
                {
                    this.testProgressLastName = value;
                    NotifyPropertyChanged("TestProgressLastName");
                }
            }
        }

        public int TestProgressMiddleName
        {
            get { return testProgressMiddleName; }
            set
            {
                if (value != this.testProgressMiddleName)
                {
                    this.testProgressMiddleName = value;
                    NotifyPropertyChanged("TestProgressMiddleName");
                }
            }
        }

        public int TestProgressStudentID
        {
            get { return testProgressStudentID; }
            set
            {
                if (value != this.testProgressStudentID)
                {
                    this.testProgressStudentID = value;
                    NotifyPropertyChanged("TestProgressStudentID");
                }
            }
        }

        public ICommand BrowseCommand
        {
            get;
            set;
        }

        public ICommand ImportCommand
        {
            get;
            set;
        }

        public ICommand CancelCommand
        {
            get;
            set;
        }

        public ICommand NextCommand
        {
            get;
            private set;
        }
    }
}
