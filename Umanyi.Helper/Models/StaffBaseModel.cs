using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class StaffBaseModel : ModelBase
    {
        private int staffId;
        private string nameOfStaff;
        public StaffBaseModel()
        {
            StaffID = 0;
            Name = "";
        }

        public int StaffID
        {
            get { return this.staffId; }

            set
            {
                if (value != this.staffId)
                {
                    this.staffId = value;
                    NotifyPropertyChanged("StaffID");
                }
            }
        }

        public string Name
        {
            get { return this.nameOfStaff; }

            set
            {
                if (value != this.nameOfStaff)
                {
                    this.nameOfStaff = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public override void Reset()
        {
            StaffID = 0;
            Name = "";
        }
    }
}
