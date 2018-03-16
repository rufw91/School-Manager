using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.Modules.Library.Models
{
    public class UnreturnedBookModel:BookModel
    {
        public UnreturnedBookModel()
        {
            UnreturnedCopies = 0;
        }
        public decimal UnreturnedCopies
        { get; set; }
    }
}
