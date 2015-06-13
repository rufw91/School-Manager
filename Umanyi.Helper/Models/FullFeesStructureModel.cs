using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Models
{
    public class FullFeesStructureModel:ObservableCollection<FeesStructureModel>
    {
        public FullFeesStructureModel()
        {

        }

        public FullFeesStructureModel(IEnumerable<FeesStructureModel> newEntries)
            :base(newEntries)
        {

        }
    }
}
