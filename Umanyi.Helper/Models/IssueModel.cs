using System;
using System.Collections.ObjectModel;

namespace Helper.Models
{
    public class IssueModel:ModelBase
    {
        private ObservableCollection<ItemIssueModel> items;
        private int issueID;
        private DateTime dateIssued;
        private bool isCancelled;
        private string description;
        public IssueModel()
        {
            Items = new ObservableCollection<ItemIssueModel>();
            DateIssued = DateTime.Now;
            IsCancelled = false;
            Description = "";
            IssueID = 0;
        }
        public ObservableCollection<ItemIssueModel> Items
        {
            get { return this.items; }

            set
            {
                if (value != this.items)
                {
                    this.items = value;
                    NotifyPropertyChanged("Items");
                }
            }
        }

        public int IssueID
        {
            get { return this.issueID; }

            set
            {
                if (value != this.issueID)
                {
                    this.issueID = value;
                    NotifyPropertyChanged("IssueID");
                }
            }
        }

        public DateTime DateIssued
        {
            get { return this.dateIssued; }

            set
            {
                if (value != this.dateIssued)
                {
                    this.dateIssued = value;
                    NotifyPropertyChanged("DateIssued");
                }
            }
        }

        public bool IsCancelled
        {
            get { return this.isCancelled; }

            set
            {
                if (value != this.isCancelled)
                {
                    this.isCancelled = value;
                    NotifyPropertyChanged("IsCancelled");
                }
            }
        }

        public string Description
        {
            get { return this.description; }

            set
            {
                if (value != this.description)
                {
                    this.description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public override void Reset()
        {
            Items = new ObservableCollection<ItemIssueModel>();
            IssueID = 0;
            DateIssued = DateTime.Now;
            IsCancelled = true;
            Description = "";
        }
    }
}
