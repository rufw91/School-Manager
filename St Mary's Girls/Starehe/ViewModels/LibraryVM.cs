using Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class LibraryVM : ParentViewModel
    {
        public LibraryVM()
        {
            Title = "LIBRARY";

            TryAddChild(typeof(IssueBookVM));
            TryAddChild(typeof(BookReturnVM));
            TryAddChild(typeof(NewBookVM));
            TryAddChild(typeof(ModifyBookVM));            
            TryAddChild(typeof(UnreturnedBooksVM));
            TryAddChild(typeof(ViewBooksVM));
            TryAddChild(typeof(PriceListsVM));
        }
    }
}
