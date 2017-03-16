using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.Modules.Library.Models
{
    public class AllUnreturnedBooksModel : ObservableCollection<UnreturnedBookModel>
    {
        public AllUnreturnedBooksModel()
        {
                
        }
        public AllUnreturnedBooksModel(IEnumerable<UnreturnedBookModel> newEntries)
            : base(newEntries)
        {
            
        }
    }
}
