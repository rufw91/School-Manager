

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;
using UmanyiSMS.Modules.Fees.Controller;
using UmanyiSMS.Modules.Fees.Models;
using UmanyiSMS.Modules.Institution.Models;

namespace UmanyiSMS.Modules.Fees.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class ViewFullFeesStructureVM:ViewModelBase
    {
        private FixedDocument document;
        private TermModel selectedTerm;
        private ObservableCollection<TermModel> allTerms;

        public ViewFullFeesStructureVM()
        {
            InitVars();
            CreateCommands();
        }
        protected async override void InitVars()
        {
            Title = "VIEW FULL FEES STRUCTURE";
            AllTerms = await Institution.Controller.DataController.GetAllTermsAsync();
        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
             {
                 var f = await DataController.GetFullFeesStructure(selectedTerm.StartDate.AddDays(1));
                 Document = DocumentHelper.GenerateDocument(new FullFeesStructureModel(f));
             }, o => selectedTerm != null);

            FullPreviewCommand = new RelayCommand(async o =>
            {
                var f = await DataController.GetFullFeesStructure(selectedTerm.StartDate.AddDays(1));
                var xdc = DocumentHelper.GenerateDocument(new FullFeesStructureModel(f));
                if (ShowFullPreviewAction != null)
                    ShowFullPreviewAction.Invoke(xdc);
            }, o => selectedTerm != null);
        }

        public Action<FixedDocument> ShowFullPreviewAction
        { get; set; }

        public ICommand GenerateCommand
        {
            get;
            private set;
        }

        public ICommand FullPreviewCommand
        {
            get;
            private set;
        }

        public ObservableCollection<TermModel> AllTerms
        {
            get { return allTerms; }
            private set
            {
                if (allTerms != value)
                {
                    allTerms = value;
                    NotifyPropertyChanged("AllTerms");
                }
            }
        }

        public TermModel SelectedTerm
        {
            get { return selectedTerm; }
            set
            {
                if (selectedTerm != value)
                {
                    selectedTerm = value;
                    NotifyPropertyChanged("SelectedTerm");
                }
            }
        }

        public FixedDocument Document
        {
            get { return document; }
            private set
            {
                if (document != value)
                {
                    document = value;
                    NotifyPropertyChanged("Document");
                }
            }
        }

        public override void Reset()
        {
            
        }
    }
}
