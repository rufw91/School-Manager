using Helper.Models;
using System.Collections.ObjectModel;

namespace Helper
{
    public class ComparisonCollection:ObservableCollection<ComparisonModel>
    {
        public ComparisonCollection()
        {
            Add(new ComparisonModel("None", 0));
            Add(new ComparisonModel("Is Equal To", 1));
            Add(new ComparisonModel("Is Greater Than", 2));
            Add(new ComparisonModel("Is Greater Than or Equal To", 3));
            Add(new ComparisonModel("Is Less Than", 4));
            Add(new ComparisonModel("Is Less Than or Equal To", 5));
            
        }
    }
}
