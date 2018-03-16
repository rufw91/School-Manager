using UmanyiSMS.Lib.Presentation;
using System;
using System.Threading.Tasks;

namespace UmanyiSMS.Lib.Controllers
{
    public abstract class SyncHelper
    {
        public abstract Task Sync(IProgress<SyncOperationProgress> progressReporter);
    }
}
