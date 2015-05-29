using Helper.Models;
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
                    return succ;
                });
        }

    }
}
