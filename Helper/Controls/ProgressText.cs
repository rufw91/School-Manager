using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Helper.Controls
{
    public class ProgressText: Control
    {
        public static readonly DependencyProperty TaskStateProperty = DependencyProperty.Register("TaskState",
            typeof(TaskStates), typeof(ProgressText), new PropertyMetadata(TaskStates.Idle));
               
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", 
            typeof(string), typeof(ProgressText), new PropertyMetadata(""));

        public TaskStates TaskState
        {
            get { return (TaskStates)GetValue(TaskStateProperty); }
            set
            {
                SetValue(TaskStateProperty, value);
            }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
    }

}
