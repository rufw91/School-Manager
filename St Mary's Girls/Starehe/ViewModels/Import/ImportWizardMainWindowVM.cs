using Helper;
using Helper.Models;
using OpenXmlPackaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
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
        string genderColumn;
        string classColumn;
        string phoneNoColumn;
        string balanceBFColumn;
        string boardingColumn;
        string dateOfBirthColumn;
        string dateOfAdmissionColumn;
        string nameOfGuardianColumn;
        string addressColumn;
        string cityColumn;
        string kcpeScoreColumn;
        
        private int testProgressStudentID;
        private int testProgressFirstName;
        private int testProgressMiddleName;
        private int testProgressLastName;
        private int testProgressGender;
        private int testProgressClass;
        private int testProgressPhoneNo;
        private int testProgressBalanceBF;
        private int testProgressBoarding;
        private int testProgressDateOfBirth;
        private int testProgressDateOfAdmission;
        private int testProgressNameOfGuardian;
        private int testProgressAddress;
        private int testProgressCity;
        private int testProgressKCPEScore;
        
        private bool hasFinished;
        private int progress;
        private string progressText;
        private string informationText;
        private string bedNoColumn;
        private string dormitoryColumn;

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
            AllDorms = new List<DormModel>(await DataAccess.GetAllDormsAsync());

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
                    Progress = Convert.ToInt32(((double)((i +1)* 100) / (double)data.Rows.Count));
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
                    Progress = Convert.ToInt32(((double)((i+1) * 100) / (double)newClasses.Count));
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
                AllClasses = new List<ClassModel>(DataAccess.GetAllClassesAsync().Result);
                Progress = 100;

                if (!string.IsNullOrWhiteSpace(dormitoryColumn))
                {
                    ProgressText = "Checking for New Dormitories.";
                    List<string> newDorms = new List<string>();
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        DataRow dtr = data.Rows[i];
                        Progress = Convert.ToInt32(((double)((i + 1) * 100) / (double)data.Rows.Count));
                        if (Errors.ContainsKey(i))
                            continue;

                        string newDorm = dtr[dormitoryColumn].ToString();
                        if ((!newDorms.Any(s => s.ToUpper() == newDorm.ToUpper())) && (!AllDorms.Any(v => v.NameOfDormitory.ToUpper() == newDorm.ToUpper())))
                            newDorms.Add(newDorm.ToUpper());
                    }

                    ProgressText = "Saving new Dorms.";
                    DormModel dm;
                    for (int i = 0; i < newDorms.Count; i++)
                    {
                        Progress = Convert.ToInt32(((double)((i + 1) * 100) / (double)newClasses.Count));
                        dm = new DormModel(0, newDorms[i]);
                        await DataAccess.SaveNewDormitory(dm);
                    }
                }

                Progress = 0;
                ProgressText = "Refreshing Classes.";
                AllClasses = new List<ClassModel>(DataAccess.GetAllClassesAsync().Result);
                Progress = 100;

                Progress = 0;
                ProgressText = "Refreshing Dorms.";
                AllDorms = new List<DormModel>(DataAccess.GetAllDormsAsync().Result);
                Progress = 100;

                  
                StudentModel student = new StudentModel();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    DataRow dtr = data.Rows[i];
                    Progress = Convert.ToInt32(((double)((i+1) * 100) / (double)data.Rows.Count));
                    if (Errors.ContainsKey(i))
                        continue;
                    student.StudentID = int.Parse(dtr[studentIDColumn].ToString());
                    student.FirstName = dtr[firstNameColumn].ToString();
                    student.MiddleName = dtr[middleNameColumn].ToString();
                    student.LastName = dtr[lastNameColumn].ToString();
                    ProgressText = "Saving Student:  " + student.NameOfStudent;
                    student.Gender = ConvertGender(dtr[genderColumn].ToString());
                    student.ClassID = AllClasses.First(c => c.NameOfClass.ToUpper() == dtr[classColumn].ToString().ToUpper()).ClassID;
                    if (!string.IsNullOrWhiteSpace(dateOfBirthColumn))
                    {

                        DateTime d1;
                        ConvertDate(dtr[dateOfBirthColumn].ToString(), out d1);
                        student.DateOfBirth = d1;
                    }
                    if (!string.IsNullOrWhiteSpace(dateOfAdmissionColumn))
                    {
                        DateTime d;
                        ConvertDate(dtr[dateOfAdmissionColumn].ToString(), out d);
                        student.DateOfAdmission = d;
                    }
                    
                    student.Email = "test@example.com";
                    student.GuardianPhoneNo = string.IsNullOrWhiteSpace(phoneNoColumn) ? "-" : dtr[phoneNoColumn].ToString();
                    student.NameOfGuardian = string.IsNullOrWhiteSpace(nameOfGuardianColumn) ? "-" : dtr[nameOfGuardianColumn].ToString();
                    student.PostalCode = "X";
                    student.PrevBalance = string.IsNullOrWhiteSpace(balanceBFColumn) ? 0.0m : decimal.Parse(dtr[balanceBFColumn].ToString());
                    student.IsBoarder = string.IsNullOrWhiteSpace(boardingColumn) ? false : ConvertBoadingString(dtr[boardingColumn].ToString());
                    student.KCPEScore = string.IsNullOrWhiteSpace(kcpeScoreColumn) ? 0 : int.Parse(dtr[kcpeScoreColumn].ToString());

                    student.Address = string.IsNullOrWhiteSpace(addressColumn) ? "-" : dtr[addressColumn].ToString();
                    if (student.IsBoarder)
                    {                        
                        student.DormitoryID = (string.IsNullOrWhiteSpace(dormitoryColumn) || AllDorms.Any(c => c.NameOfDormitory.ToUpper() ==
                            dtr[dormitoryColumn].ToString().ToUpper())) ? 0 : AllDorms.First(c => c.NameOfDormitory.ToUpper() ==
                                dtr[dormitoryColumn].ToString().ToUpper()).DormitoryID;
                        student.BedNo = (string.IsNullOrWhiteSpace(bedNoColumn) || student.DormitoryID == 0) ? "" : dtr[bedNoColumn].ToString();
                    }
                    await DataAccess.SaveNewStudentAsync(student);
                }

                ProgressText = "Successfully saved Students.";
                HasFinished = true;
                InformationText = "Successfully completed Import Process.";
            });
        }

        private Gender ConvertGender(string value)
        {
            if (string.IsNullOrWhiteSpace(value.Trim()))
                return Gender.Male;
            else if (value.Trim().ToUpper()[0] == 'M')
                return Gender.Male;
            else if (value.Trim().ToUpper()[0] == 'F')
                return Gender.Female;
            else return Gender.Male;
        }

        private bool ConvertBoadingString(string text)
        {
            text = text.Trim().ToUpper();
            if ((text.Contains("BOARD")||text.Contains("B")) && !text.Contains("NOT"))
                return true;
            if ((text.Contains("DAY")||text.Contains("D")) && !text.Contains("NOT"))
                return false;
            return false;
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
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        dtr[i] = dtr[i].ToString().Replace("'", "").Trim().Replace("@","")
                            .Replace("#","")
                            .Replace(":", "")
                            .Replace("\"", "")
                            .Replace("\\", "")
                            .Replace("?", "")
                            .Replace("!", "")
                            .Replace("$", "")
                            .Replace("%", "")
                            .Replace("^", "")
                        .Replace(".", "");
                        if (string.IsNullOrWhiteSpace(dtr[i].ToString()))
                            dtr[i] = "-";
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
                bool stringsAreNull = string.IsNullOrWhiteSpace(studentIDColumn) ||
                    string.IsNullOrWhiteSpace(firstNameColumn) || string.IsNullOrWhiteSpace(middleNameColumn) ||
                    string.IsNullOrWhiteSpace(lastNameColumn) || string.IsNullOrWhiteSpace(genderColumn) ||
                    string.IsNullOrWhiteSpace(classColumn) || string.IsNullOrWhiteSpace(phoneNoColumn);

                bool prog100 = testProgressStudentID == 100 && testProgressFirstName == 100 && testProgressMiddleName == 100
                    && testProgressLastName == 100 && testProgressGender == 100 && testProgressClass == 100 && testProgressPhoneNo == 100;
                return !stringsAreNull && prog100;
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

        public string GenderColumn
        {
            get { return genderColumn; }
            set
            {
                if (value != this.genderColumn)
                {
                    this.genderColumn = value;
                    NotifyPropertyChanged("GenderColumn");
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

        public string NameOfGuardianColumn
        {
            get { return nameOfGuardianColumn; }
            set
            {
                if (value != this.nameOfGuardianColumn)
                {
                    this.nameOfGuardianColumn = value;
                    NotifyPropertyChanged("NameOfGuardianColumn");
                    TestAll();
                }
            }
        }

        public string BoardingColumn
        {
            get { return boardingColumn; }
            set
            {
                if (value != this.boardingColumn)
                {
                    this.boardingColumn = value;
                    NotifyPropertyChanged("BoardingColumn");
                    TestAll();
                }
            }
        }

        public string DateOfBirthColumn
        {
            get { return dateOfBirthColumn; }
            set
            {
                if (value != this.dateOfBirthColumn)
                {
                    this.dateOfBirthColumn = value;
                    NotifyPropertyChanged("DateOfBirthColumn");
                    TestAll();
                }
            }
        }

        public string DateOfAdmissionColumn
        {
            get { return dateOfAdmissionColumn; }
            set
            {
                if (value != this.dateOfAdmissionColumn)
                {
                    this.dateOfAdmissionColumn = value;
                    NotifyPropertyChanged("DateOfAdmissionColumn");
                    TestAll();
                }
            }
        }

        public string AddressColumn
        {
            get { return addressColumn; }
            set
            {
                if (value != this.addressColumn)
                {
                    this.addressColumn = value;
                    NotifyPropertyChanged("AddressColumn");
                    TestAll();
                }
            }
        }

        public string CityColumn
        {
            get { return cityColumn; }
            set
            {
                if (value != this.cityColumn)
                {
                    this.cityColumn = value;
                    NotifyPropertyChanged("CityColumn");
                    TestAll();
                }
            }
        }

        public string KCPEScoreColumn
        {
            get { return kcpeScoreColumn; }
            set
            {
                if (value != this.kcpeScoreColumn)
                {
                    this.kcpeScoreColumn = value;
                    NotifyPropertyChanged("KCPEScoreColumn");
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
            await Task.WhenAll<bool>(
                TestColumnAsync(studentIDColumn, "Student ID", typeof(int), false, false, OnProgressChanged, "TestProgressStudentID"),
                TestColumnAsync(firstNameColumn, "First Name", typeof(string), false, false,OnProgressChanged, "TestProgressFirstName"),
                TestColumnAsync(middleNameColumn, "Middle Name", typeof(string), false, false,OnProgressChanged, "TestProgressMiddleName"),
                TestColumnAsync(lastNameColumn, "Last Name", typeof(string), false, false, OnProgressChanged,"TestProgressLastName"),
                TestColumnAsync(genderColumn, "Gender", typeof(string), false, false, OnProgressChanged, "TestProgressGender"),
                TestColumnAsync(classColumn, "Class", typeof(string), false, false, OnProgressChanged,"TestProgressClass"),
                TestColumnAsync(phoneNoColumn, "Phone No", typeof(string), false, false, OnProgressChanged,"TestProgressPhoneNo"),
                TestColumnAsync(balanceBFColumn, "Previous Fees Balance", typeof(decimal), false, true,OnProgressChanged, "TestProgressBalanceBF"),
                TestColumnAsync(nameOfGuardianColumn, "Name of Guardian", typeof(string), false, false, OnProgressChanged,"TestProgressNameOfGuardian"),
                TestColumnAsync(dateOfBirthColumn, "Date Of Birth", typeof(DateTime), false, false, OnProgressChanged,"TestProgressDateOfBirth"),
                TestColumnAsync(dateOfAdmissionColumn, "Date Of Admission", typeof(DateTime), false, false, OnProgressChanged,"TestProgressDateOfAdmission"),
                TestColumnAsync(boardingColumn, "Boarding", typeof(string), true, false, OnProgressChanged, "TestProgressBoarding"),
                TestColumnAsync(kcpeScoreColumn, "KCPE Score", typeof(int), true, false, OnProgressChanged, "TestProgressKCPEScore"),                
                TestColumnAsync(addressColumn, "Address", typeof(string), true, false, OnProgressChanged, "TestProgressAddress"),
                TestColumnAsync(cityColumn, "City", typeof(string), true, false, OnProgressChanged, "TestProgressCity"));
        }

        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch(e.UserState.ToString())
            {
                case "TestProgressStudentID": TestProgressStudentID = e.ProgressPercentage; break;
                case "TestProgressFirstName": TestProgressFirstName = e.ProgressPercentage; break;
                case "TestProgressMiddleName": TestProgressMiddleName = e.ProgressPercentage; break;
                case "TestProgressLastName": TestProgressLastName = e.ProgressPercentage; break;
                case "TestProgressClass": TestProgressClass = e.ProgressPercentage; break;
                case "TestProgressPhoneNo": TestProgressPhoneNo = e.ProgressPercentage; break;
                case "TestProgressBalanceBF": TestProgressBalanceBF = e.ProgressPercentage; break;
                case "TestProgressNameOfGuardian": TestProgressNameOfGuardian = e.ProgressPercentage; break;
                case "TestProgressDateOfBirth": TestProgressDateOfBirth = e.ProgressPercentage; break;
                case "TestProgressDateOfAdmission": TestProgressDateOfAdmission = e.ProgressPercentage; break;
                case "TestProgressBoarding": TestProgressBoarding = e.ProgressPercentage; break;
                case "TestProgressKCPEScore": TestProgressKCPEScore = e.ProgressPercentage; break;
                case "TestProgressGender": TestProgressGender = e.ProgressPercentage; break;
                case "TestProgressAddress": TestProgressAddress = e.ProgressPercentage; break;
                case "TestProgressCity": TestProgressCity = e.ProgressPercentage; break;
            }
        }

        private async Task<bool> TestColumnAsync(string columnName, string friendlyName, Type dataType, bool allowEmptyStrings, bool allowZeroAndNegativeValues,
            EventHandler<ProgressChangedEventArgs> progressChangedCallBack, string nameOfProperty)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                TestProgressAddress = 100;
                return false;
            }
            return await Task.Run<bool>(() => TestColumn(columnName, friendlyName, dataType, allowEmptyStrings, allowZeroAndNegativeValues, 
                progressChangedCallBack, nameOfProperty));
        }

        private bool TestColumn(string columnName, string friendlyName, Type dataType, bool allowEmptyStrings, bool allowZeroAndNegativeValues,
            EventHandler<ProgressChangedEventArgs> progressChangedCallBack, string nameOfProperty)
        {
            int progressValue = 0;
                bool succ = true;
                bool isOk;
                for (int i = 0; i < data.Rows.Count; i++)
                {

                    progressValue = Convert.ToInt32(((double)((i + 1) * 100) / (double)data.Rows.Count));
                    progressChangedCallBack.Invoke(this, new ProgressChangedEventArgs(progressValue, nameOfProperty));
                    isOk = !string.IsNullOrWhiteSpace(data.Rows[i][columnName].ToString());
                    if (allowEmptyStrings)
                        isOk = true;
                    if (dataType == typeof(int))
                    {
                        int dt = 0;
                        isOk = int.TryParse(data.Rows[i][columnName].ToString(), out dt) && (allowZeroAndNegativeValues ? true : dt > 0);
                    }
                    else if (dataType == typeof(DateTime))
                    {
                        DateTime dt;
                        if (nameOfProperty == "TestProgressDateOfBirth")
                        isOk = ConvertDate(data.Rows[i][dateOfBirthColumn].ToString(), out dt);
                        else
                            isOk = ConvertDate(data.Rows[i][dateOfAdmissionColumn].ToString(), out dt);
                    }
                    else if (dataType == typeof(decimal))
                    {
                        decimal dt;
                        isOk = decimal.TryParse(data.Rows[i][columnName].ToString(), out dt);
                    }
                    succ = succ && isOk;
                    if (!isOk)
                        AddError(i, "The value [" + data.Rows[i][columnName] + "] at " + friendlyName + " for the Student [" + data.Rows[i][studentIDColumn] + "] is invalid.");
                }
                return succ;
        }

        public int TestProgressKCPEScore
        {
            get { return testProgressKCPEScore; }
            set
            {
                if (value != this.testProgressKCPEScore)
                {
                    this.testProgressKCPEScore = value;
                    NotifyPropertyChanged("TestProgressKCPEScore");
                }
            }
        }
        
        public int TestProgressCity
        {
            get { return testProgressCity; }
            set
            {
                if (value != this.testProgressCity)
                {
                    this.testProgressCity = value;
                    NotifyPropertyChanged("TestProgressCity");
                }
            }
        }
        
        public int TestProgressAddress
        {
            get { return testProgressAddress; }
            set
            {
                if (value != this.testProgressAddress)
                {
                    this.testProgressAddress = value;
                    NotifyPropertyChanged("TestProgressAddress");
                }
            }
        }

        public int TestProgressNameOfGuardian
        {
            get { return testProgressNameOfGuardian; }
            set
            {
                if (value != this.testProgressNameOfGuardian)
                {
                    this.testProgressNameOfGuardian = value;
                    NotifyPropertyChanged("TestProgressNameOfGuardian");
                }
            }
        }

        public int TestProgressDateOfBirth
        {
            get { return testProgressDateOfBirth; }
            set
            {
                if (value != this.testProgressDateOfBirth)
                {
                    this.testProgressDateOfBirth = value;
                    NotifyPropertyChanged("TestProgressDateOfBirth");
                }
            }
        }

        public int TestProgressBoarding
        {
            get { return testProgressBoarding; }
            set
            {
                if (value != this.testProgressBoarding)
                {
                    this.testProgressBoarding = value;
                    NotifyPropertyChanged("TestProgressBoarding");
                }
            }
        }

        public int TestProgressDateOfAdmission
        {
            get { return testProgressDateOfAdmission; }
            set
            {
                if (value != this.testProgressDateOfAdmission)
                {
                    this.testProgressDateOfAdmission = value;
                    NotifyPropertyChanged("TestProgressDateOfAdmission");
                }
            }
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

        public int TestProgressGender
        {
            get { return testProgressGender; }
            set
            {
                if (value != this.testProgressGender)
                {
                    this.testProgressGender = value;
                    NotifyPropertyChanged("TestProgressGender");
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

        public bool ConvertDate(string date,out DateTime returnDate)
        {
            DateTime dt;
            int i;
            if (DateTime.TryParse(date, new CultureInfo("en-GB"), DateTimeStyles.None, out dt))
            {
                returnDate = dt;
                return true;
            }
            else if (int.TryParse(date, out i))
            {
                if (i>3000)
                {
                    dt = new DateTime(1899, 12, 31).AddDays(i - 1);
                    returnDate= dt;
                    return true;
                }
                else
                {
                    returnDate = new DateTime(i,1,1);
                    return true;                     
                }
            }
            else
            returnDate = DateTime.Now;
            return true;
        }

        public List<DormModel> AllDorms { get; set; }
    }
}
