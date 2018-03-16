using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UmanyiSMS.Lib.Controllers;

namespace UmanyiSMS.Lib.Models
{
    public class PrinterHolderModel : ModelBase
    {
        private string name;
        private PrintHelper.PrinterLocation location;
        private bool isDefault;
        private string description;
        private int status;

        public PrinterHolderModel()
        {

        }

        public override void Reset()
        {
            Name = "";
            Description = "";
            Location = PrintHelper.PrinterLocation.Local;
            IsDefault = false;
            Status = 3;
        }

        public PrintHelper.PrinterLocation Location
        {
            get
            {
                return this.location;
            }
            set
            {
                if (this.location != value)
                {
                    this.location = value;
                    base.NotifyPropertyChanged("Location");
                }
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    base.NotifyPropertyChanged("Name");
                }
            }
        }

        public bool IsDefault
        {
            get
            {
                return this.isDefault;
            }
            set
            {
                if (this.isDefault != value)
                {
                    this.isDefault = value;
                    base.NotifyPropertyChanged("IsDefault");
                }
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    base.NotifyPropertyChanged("Description");
                }
            }
        }

        public int Status
        {
            get
            {
                return this.status;
            }
            set
            {
                if (this.status != value)
                {
                    this.status = value;
                    base.NotifyPropertyChanged("Status");
                }
            }
        }
    }
}
