using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Presentation
{
    public interface IAccountBase
    {
        string Name
        { get; set; }
        int AccountID
        { get; set; }
        decimal Balance
        { get; set; }

        AccountType AccountType
        { get; set; }
    }
}
