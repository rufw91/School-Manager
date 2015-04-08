using Helper.Models;
using Intuit.Ipp.Core;
using Intuit.Ipp.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class QBSyncHelper
    {
        public static Task<bool> SyncInvoice(SaleModel studenInvoice)
        {
            return Task.Run<bool>(() =>
                {
                    bool succ = true;
                    OAuthRequestValidator requestValidator = GetValidator();
                    ServiceContext context = new ServiceContext("Starehe",IntuitServicesType.QBD, requestValidator);
                    return succ;
                });
        }

        private static OAuthRequestValidator GetValidator()
        {
            OAuthRequestValidator oav = new OAuthRequestValidator("", "","","");
            return oav;
        }
    }
}
