using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Helper.Controls
{
    public class HomePageButton : Button
    {      
        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string),
            typeof(HomePageButton), new PropertyMetadata(""));

        public static readonly DependencyProperty CaptionBackgroundProperty = DependencyProperty.Register("CaptionBackground",
            typeof(Brush),typeof(HomePageButton), new PropertyMetadata(Brushes.Transparent));
        public HomePageButton()
        {
        }
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public Brush CaptionBackground
        {
            get { return (Brush)GetValue(CaptionBackgroundProperty); }
            set { SetValue(CaptionBackgroundProperty, value); }
        }
    
    }
}
