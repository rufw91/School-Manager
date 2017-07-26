using System;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.Projects.Models
{
    public class ProjectBaseModel : ModelBase
    {
        private string name;

        private int projectID;

        public int ProjectID
        {
            get
            {
                return this.projectID;
            }
            set
            {
                if (value != this.projectID)
                {
                    this.projectID = value;
                    base.NotifyPropertyChanged("ProjectID");
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
                if (value != this.name)
                {
                    this.name = value;
                    base.NotifyPropertyChanged("Name");
                }
            }
        }

        public ProjectBaseModel()
        {
            this.ProjectID = 0;
            this.Name = "";
        }

        public override void Reset()
        {
            this.ProjectID = 0;
            this.Name = "";
        }
    }
}
