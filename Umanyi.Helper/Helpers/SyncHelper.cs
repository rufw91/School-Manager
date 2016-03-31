using Helper.Presentation;
using System;
using System.Threading.Tasks;

namespace Helper
{
    public abstract class SyncHelper
    {
        public abstract Task Sync(IProgress<SyncOperationProgress> progressReporter);
    }
}
