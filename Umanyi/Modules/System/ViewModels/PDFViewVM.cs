﻿using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.System.ViewModels
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
