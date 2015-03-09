using Helper;
using Helper.Models;
using System;
using System.Collections.ObjectModel;
using System.Security.Permissions;
using System.Windows.Documents;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class ClassReportFormsVM: ViewModelBase
    {
        private bool classHasResults;
        private int selectedClassID;
        private ObservableCollection<StudentTranscriptModel> classTranscripts;
        private FixedDocument fd;
        public ClassReportFormsVM()
        {
            InitVars();
            CreateCommands();

        }
        protected async override void InitVars()
        {
            Title = "CLASS REPORT FORMS";
            SelectedClassID = 0;
            PropertyChanged += async (o, e) =>
                {
                    if (e.PropertyName == "SelectedClassID")
                    {
                        if (selectedClassID == 0)
                        {
                            classHasResults = false;
                            return;
                        }
                        ClassTranscripts = await DataAccess.GetClassTranscriptsAsync(selectedClassID);
                        classHasResults = classTranscripts.Count > 0;
                    }
                };
            AllClasses = await DataAccess.GetAllClassesAsync();
            NotifyPropertyChanged("AllClasses");
        }
        public int SelectedClassID
        {
            get { return selectedClassID; }
            set
            {
                if (value != selectedClassID)
                {
                    selectedClassID = value;
                    NotifyPropertyChanged("SelectedClassID");

                }
            }
        }

        public ObservableCollection<StudentTranscriptModel> ClassTranscripts
        {
            get { return classTranscripts; }
            set
            {
                if (value != classTranscripts)
                {
                    classTranscripts = value;
                    NotifyPropertyChanged("ClassTranscripts");

                }
            }
        }

        protected override void CreateCommands()
        {
           GenerateCommand=new RelayCommand(o=>
            {
                ClassTranscriptsModel cs = new ClassTranscriptsModel() { Entries = classTranscripts };
                Document = DocumentHelper.GenerateDocument(cs);
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(Document);
            },o=>CanGenerate());
        }

        private bool CanGenerate()
        {
            return classHasResults;
        }

        private FixedDocument Document
        {
            get { return this.fd; }

            set
            {
                if (value != this.fd)
                {
                    this.fd = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }

        public ICommand GenerateCommand
        {
            get;
            private set;
        }

        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }

        public override void Reset()
        {
            
        }

        public ObservableCollection<ClassModel> AllClasses { get; set; }
    }
}
