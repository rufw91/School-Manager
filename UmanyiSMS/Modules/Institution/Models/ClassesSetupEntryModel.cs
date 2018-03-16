using System;

namespace UmanyiSMS.Modules.Institution.Models
{
    public class ClassesSetupEntryModel : ClassModel
    {
        public int ClassSetupID
        {
            get;
            set;
        }

        public ClassesSetupEntryModel()
        {
            this.ClassSetupID = 0;
        }

        public ClassesSetupEntryModel(ClassModel classModel)
        {
            this.ClassSetupID = 0;
            base.ClassID = classModel.ClassID;
            base.NameOfClass = classModel.NameOfClass;
        }

        public override void Reset()
        {
            this.ClassSetupID = 0;
        }
    }
}
