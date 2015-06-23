using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class ViewFullFeesStructureVM:ViewModelBase
    {
        private DateTime currentDate;
        private FixedDocument document;

        public ViewFullFeesStructureVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "VIEW FULL FEES STRUCTURE";
            CurrentDate = DateTime.Now;
        }

        protected override void CreateCommands()
        {
            GenerateCommand = new RelayCommand(async o =>
             {
                 var f =await DataAccess.GetFullFeesStructure(DateTime.Now);
                 Document = DocumentHelper.GenerateDocument(new FullFeesStructureModel( f ));
             },o => true);

            FullPreviewCommand = new RelayCommand(async o =>
            {
                var f = await DataAccess.GetFullFeesStructure(DateTime.Now);
                var xdc = DocumentHelper.GenerateDocument(new FullFeesStructureModel( f ));
                if (ShowFullPreviewAction != null)
                    ShowFullPreviewAction.Invoke(xdc);
            }, o => true);
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

        public DateTime CurrentDate
        {
            get { return currentDate; }
            set
            {
                if (currentDate != value)
                {
                    currentDate = value;
                    NotifyPropertyChanged("CurrentDate");
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
