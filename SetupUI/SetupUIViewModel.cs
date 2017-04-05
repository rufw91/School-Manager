using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetupUI
{
    public class SetupUIViewModel
    {
        private SetupUIModel model;

        public SetupUIViewModel()
        { }

        public int CurrentComponentProgressPercentage { get; internal set; }
        public string CurrentlyProcessingPackageName { get; set; }
        public int OverallProgressPercentage { get; internal set; }
    }
}
