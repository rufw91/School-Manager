using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Helper.Controls
{
    public class HomePageButton : Button
    {      
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string),
            typeof(HomePageButton), new PropertyMetadata(""));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description",
            typeof(string),typeof(HomePageButton), new PropertyMetadata(""));
        public HomePageButton()
        {
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
    
    }
}
