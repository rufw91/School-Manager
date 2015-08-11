using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Presentation
{
    public class SyncOperationProgress
    {
        public int CurrentItem { get; set; }
        public decimal Percentage { get; set; }
        public bool Succeeded { get; set; }
        public bool Completed { get; set; }
        public SyncOperationProgress()
        { }
    }
}
