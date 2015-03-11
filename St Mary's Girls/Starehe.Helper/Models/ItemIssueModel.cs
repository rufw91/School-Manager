using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ItemIssueModel:ModelBase
    {
        private int issueID;
        private DateTime dateIssued;
        private ObservableCollection<ItemIssueEntryModel> entries;
        private string description;
        public ItemIssueModel()
        {
            IssueID = 0;
            Description = "";
            DateIssued = DateTime.Now;
            Entries = new ObservableCollection<ItemIssueEntryModel>();
        }
        public override void Reset()
        {
            IssueID = 0;
            Description = "";
            DateIssued = DateTime.Now;
            Entries.Clear();
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
            get { return dateIssued; }

            set
            {
                if (value != this.dateIssued)
                {
                    this.dateIssued = value;
                    NotifyPropertyChanged("DateIssued");
                }
            }
        }
        public ObservableCollection<ItemIssueEntryModel> Entries
        {
            get { return entries; }

            set
            {
                if (value != this.entries)
                {
                    this.entries = value;
                    NotifyPropertyChanged("Entries");
                }
            }
        }

        public string Description
        {
            get { return description; }

            set
            {
                if (value != this.description)
                {
                    this.description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }
    }
}
