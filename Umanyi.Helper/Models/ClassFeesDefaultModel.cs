using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class ClassBalancesListModel: ClassModel
    {
        public ClassBalancesListModel()
        {
            Date = DateTime.Now;
            Entries = new ObservableCollection<StudentFeesDefaultModel>();
        }
        public override void Reset()
        {
        }

        public DateTime Date { get; set; }

        public ObservableCollection<StudentFeesDefaultModel> Entries
        { get; set; }

        public decimal Total { get; set; }

        public decimal TotalUnpaid { get; set; }
    }
}
