using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper.Presentation
{
    public class Progress<T>: IProgress<T>
    {
        public Progress(T reporter)
        {

        }
        public void Report(T value)
        {
            
        }

        public Progress()
        { }

        public Progress(Action<T> handler)
        { }
       
        protected virtual void OnReport(T value)
        { }
    }
}
