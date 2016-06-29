using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Presentation
{
    public interface IAccount: IAccountBase, IList<IAccount>
    {       
        IAccount this[string accountName]
        {
            get;
        }
    }
}
