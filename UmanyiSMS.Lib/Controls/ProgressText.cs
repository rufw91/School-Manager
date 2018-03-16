using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UmanyiSMS.Lib.Controls
{
    public class ProgressText: Control
    {
        public static readonly DependencyProperty TaskStateProperty = DependencyProperty.Register("TaskState",
            typeof(TaskState), typeof(ProgressText), new PropertyMetadata(TaskState.Idle));
               
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", 
            typeof(string), typeof(ProgressText), new PropertyMetadata(""));

        public TaskState TaskState
        {
            get { return (TaskState)GetValue(TaskStateProperty); }
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
