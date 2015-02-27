using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    public class StudentTranscriptVM: ViewModelBase
    {
        StudentTranscriptModel transcript;
        FixedDocument fd;
        public StudentTranscriptVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "STUDENT TRANSCRIPT";
            Transcript = new StudentTranscriptModel();            
        }

        protected override void CreateCommands()
        {
            SaveCommand = new RelayCommand(async o => 
            {
                bool succ = await DataAccess.SaveNewStudentTranscript(transcript);
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    Reset(); 
                }
                else
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }, o => CanSave());
            SaveAndPrintCommand = new RelayCommand(async o =>
            {
                bool succ = await DataAccess.SaveNewStudentTranscript(transcript);
                if (succ)
                {
                    MessageBox.Show("Successfully saved details.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                }
                else
                    MessageBox.Show("Could not save details.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);                
                Document = DocumentHelper.GenerateDocument(transcript);
                Reset();
                if (ShowPrintDialogAction != null)
                    ShowPrintDialogAction.Invoke(Document);
            }, o => CanSave());

            RefreshCommand = new RelayCommand(o =>
            {               
                Transcript.CopyFrom(DataAccess.GetStudentTranscript(transcript).Result);
            }, o => !transcript.HasErrors);
        }

        private bool CanSave()
        {
            transcript.CheckErrors();
            return !transcript.HasErrors&&transcript.Entries.Count>0;
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

        public StudentTranscriptModel Transcript
        {
            get { return this.transcript; }

            set
            {
                if (value != this.transcript)
                {
                    this.transcript = value;
                    NotifyPropertyChanged("Transcript");
                }
            }
        }
        public Action<FixedDocument> ShowPrintDialogAction
        {
            get;
            set;
        }
        public ICommand SaveCommand
        { get; private set; }

        public ICommand RefreshCommand
        { get; private set; }
        public ICommand SaveAndPrintCommand
        { get; private set; }

        public override void Reset()
        {
            transcript.Reset();
        }
    }
}
