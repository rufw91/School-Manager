using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Helper.Models
{
    public class DormModel: ModelBase
    {
        string nameOfDormitory;
        int dormitoryID;

        public DormModel()
        {
            DormitoryID = 0;
            NameOfDormitory = "";
        }
        public DormModel(int dormID, string name) 
        {
            DormitoryID = dormID;
            NameOfDormitory = name;
        }
        
        public int DormitoryID
        {
            get { return this.dormitoryID; }

            set
            {
                if (value != this.dormitoryID)
                {
                    this.dormitoryID = value;
                    NotifyPropertyChanged("DormitoryID");
                }
            }
        }

        public string NameOfDormitory
        {
            get { return this.nameOfDormitory; }

            set
            {
                if (value != this.nameOfDormitory)
                {
                    this.nameOfDormitory = value;
                    NotifyPropertyChanged("NameOfDormitory");
                }
            }
        }

        public override void Reset()
        {
            NameOfDormitory = "";
            DormitoryID = 0;
        }
    }
}
