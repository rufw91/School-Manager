using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UmanyiSMS.Modules.Fees.Models
{
    public class VoteHeadModel:FeesStructureEntryModel
    {
        public VoteHeadModel()
        {

        }
        public VoteHeadModel(FeesStructureEntryModel d)
        {
            Name = d.Name;
            Amount = d.Amount;
        }
    }
}
