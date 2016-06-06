using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class BooksVM : ParentViewModel
    {
        public BooksVM()
        {
            Title = "BOOKS";
            TryAddChild(typeof(ReceiveBooksVM));
            TryAddChild(typeof(BookReceiptHistoryVM));
        }
    }
}
