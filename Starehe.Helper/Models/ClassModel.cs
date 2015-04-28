using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Helper.Models
{
    public class ClassModel : ModelBase
    {
        int classID;
        string nameOfClass;
        public ClassModel()
        {
            ClassID = 0;
            NameOfClass = "";
        }
        public ClassModel(int classid, string nameOfClass)
        {
            ClassID = classid;
            NameOfClass = nameOfClass;
        }
        
        public int ClassID
        {
            get { return classID; }

            set
            {
                if (value != classID)
                {
                    classID = value;
                    NotifyPropertyChanged("ClassID");
                }
            }
        }

        public string NameOfClass
        {
            get { return this.nameOfClass; }

            set
            {
                if (value != this.nameOfClass)
                {
                    this.nameOfClass = value;
                    NotifyPropertyChanged("NameOfClass");
                }
            }
        }

        public override void Reset()
        {
            ClassID = 0;
            NameOfClass = "";
        }
    }
}
