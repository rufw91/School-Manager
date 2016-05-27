using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Helper.Controls
{
    public class HomePageButton : Button
    {
        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(HomePageButton), new PropertyMetadata(""));

        public static readonly DependencyProperty CaptionBackgroundProperty = DependencyProperty.Register("CaptionBackground", typeof(Brush), typeof(HomePageButton), new PropertyMetadata(Brushes.Transparent));

        public string Caption
        {
            get
            {
                return (string)base.GetValue(HomePageButton.CaptionProperty);
            }
            set
            {
                base.SetValue(HomePageButton.CaptionProperty, value);
            }
        }

        public Brush CaptionBackground
        {
            get
            {
                return (Brush)base.GetValue(HomePageButton.CaptionBackgroundProperty);
            }
            set
            {
                base.SetValue(HomePageButton.CaptionBackgroundProperty, value);
            }
        }
    }
}
