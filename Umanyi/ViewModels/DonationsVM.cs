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
    public class DonationsVM : ParentViewModel
    {
        public DonationsVM()
        {
            base.TryAddChild(typeof(NewDonorVM));
            base.TryAddChild(typeof(NewDonationVM));
            base.TryAddChild(typeof(NewPledgeVM));
            base.TryAddChild(typeof(DonationsHistoryVM));
            base.TryAddChild(typeof(AllDonorsVM));

        }
    }
}
