using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace UmanyiSMS.Lib.Controls
{
    [TemplatePart(Name = PART_Ellipse, Type = typeof(Ellipse))]
    [TemplatePart(Name = PART_Text, Type = typeof(TextBlock))]
    [TemplatePart(Name = PART_BulletText, Type = typeof(TextBlock))]
    public class SyncIcon : Control
    {
        private const string PART_Ellipse = "PART_Ellipse";
        private const string PART_Text = "PART_Text";
        private const string PART_BulletText = "PART_BulletText";
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(SyncIcon), new PropertyMetadata(false));
        public static readonly DependencyProperty OperationSucceededProperty = DependencyProperty.Register("OperationSucceeded", typeof(bool), typeof(SyncIcon), new PropertyMetadata(false));
        public static readonly DependencyProperty OperationCompletedProperty = DependencyProperty.Register("OperationCompleted", typeof(bool), typeof(SyncIcon), new PropertyMetadata(false));
        public static readonly DependencyProperty BulletTextProperty = DependencyProperty.Register("BulletText", typeof(string), typeof(SyncIcon), new PropertyMetadata("1"));
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SyncIcon), new PropertyMetadata(""));
        
        public bool IsActive
        {
            get
            {
                return (bool)GetValue(IsActiveProperty);
            }
            set
            {
                SetValue(IsActiveProperty, value);
            }
        }

        public bool OperationSucceeded
        {
            get
            {
                return (bool)GetValue(OperationSucceededProperty);
            }
            set
            {
                SetValue(OperationSucceededProperty, value);
            }
        }

        public bool OperationCompleted
        {
            get
            {
                return (bool)GetValue(OperationCompletedProperty);
            }
            set
            {
                SetValue(OperationCompletedProperty, value);
            }
        }
        
        public string BulletText
        {
            get
            {
                return (string)GetValue(BulletTextProperty);
            }
            set
            {
                SetValue(BulletTextProperty, value);
            }
        }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
    }
}
