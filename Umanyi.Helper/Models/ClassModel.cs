using System;

namespace Helper.Models
{
    public class ClassModel : ModelBase
    {
        private int classID;

        private string nameOfClass;

        public int ClassID
        {
            get
            {
                return this.classID;
            }
            set
            {
                if (value != this.classID)
                {
                    this.classID = value;
                    base.NotifyPropertyChanged("ClassID");
                }
            }
        }

        public string NameOfClass
        {
            get
            {
                return this.nameOfClass;
            }
            set
            {
                if (value != this.nameOfClass)
                {
                    this.nameOfClass = value;
                    base.NotifyPropertyChanged("NameOfClass");
                }
            }
        }

        public ClassModel()
        {
            this.ClassID = 0;
            this.NameOfClass = "";
        }

        public ClassModel(int classid, string nameOfClass)
        {
            this.ClassID = classid;
            this.NameOfClass = nameOfClass;
        }

        public override void Reset()
        {
            this.ClassID = 0;
            this.NameOfClass = "";
        }
    }
}
