using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class AllUnreturnedBooksModel:ObservableCollection<BookModel>
    {
        public AllUnreturnedBooksModel()
        {
                
        }
        public AllUnreturnedBooksModel(IEnumerable<BookModel> newEntries)
            : base(newEntries)
        {
            
        }
    }
}
