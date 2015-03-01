using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Helpers
{
    public class ActivationHelper
    {

        public Task<bool> Activate()
        {
            return Task.Run<bool>(() => { return true; });
        }

        public Task<bool> IsActivated()
        {
            return Task.Run<bool>(() => { return true; });
        }

        private Task<bool> DeActivate()
        {
            return Task.Run<bool>(() => { return true; });
        }
    }
}
