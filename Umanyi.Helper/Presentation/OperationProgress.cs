using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Presentation
{
    public class OperationProgress
    {
        public int OverallProgress { get; private set; }
        public string ProgressText { get; private set; }
        public OperationProgress(int overallProgress, string currentFile)
        {
            OverallProgress = overallProgress;
            ProgressText = currentFile;
        }
    }
}
