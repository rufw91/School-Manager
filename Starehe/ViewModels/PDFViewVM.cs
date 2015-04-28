using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    public class PDFViewVM: ViewModelBase
    {
        public PDFViewVM(string pathToFile)
        {
            InitVars();
            CurrentPdf = pathToFile;
        }
        protected override void InitVars()
        {
            Title="PDF View";
        }

        protected override void CreateCommands()
        {
            
        }
        public string CurrentPdf
        {
            get
            {
                return currentPdf;
            }

            set
            {
                if (currentPdf != value)
                {
                    currentPdf = value;
                    NotifyPropertyChanged("CurrentPdf");
                }
            }
        }

        public override void Reset()
        {
        }

        public string currentPdf { get; set; }
    }
}
