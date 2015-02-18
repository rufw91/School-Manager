using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class DormitoryMemberModel: StudentBaseModel
    {
        private string bedNo;
        public DormitoryMemberModel()
        {
            BedNo = "";
        }
        public override void Reset()
        {
            base.Reset();
            BedNo = "";
        }

        public string BedNo
        {
            get { return this.bedNo; }

            set
            {
                if (value != this.bedNo)
                {
                    this.bedNo = value;
                    NotifyPropertyChanged("BedNo");
                }
            }
        }
    }
}
