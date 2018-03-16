using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Institution.Models
{
    public class SubjectBaseModel: ModelBase
    {
        int subjectID;
        string nameOfSubject;

        public SubjectBaseModel()
        {
            SubjectID = 0;
            NameOfSubject = "";
        }

        public SubjectBaseModel(int newsubjectID, string newNameofsubject)
        {
            SubjectID = newsubjectID;
            NameOfSubject = newNameofsubject;
        }
       
        public int SubjectID
        {
            get { return this.subjectID; }

            set
            {
                if (value != this.subjectID)
                {
                    this.subjectID = value;
                    NotifyPropertyChanged("SubjectID");
                }
            }
        }

        public string NameOfSubject
        {
            get { return this.nameOfSubject; }

            set
            {
                if (value != this.nameOfSubject)
                {
                    this.nameOfSubject = value;
                    NotifyPropertyChanged("NameOfSubject");
                }
            }
        }

        public override void Reset()
        {
            SubjectID = 0;
            NameOfSubject = "";
        }
    }
}
