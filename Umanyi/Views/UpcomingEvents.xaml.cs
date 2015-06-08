using Helper.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace UmanyiSMS.Views
{
    public partial class UpcomingEvents : UserControl
    {
        public static readonly DependencyProperty DisplayEventDetailCommandProperty =
            DependencyProperty.Register("DisplayEventDetailCommand", typeof(ICommand), typeof(UpcomingEvents),
            new PropertyMetadata(null));
        public UpcomingEvents()
        {
            InitializeComponent();
            Binding myBinding = new Binding("DataContext.DisplayEventDetailCommand");
            myBinding.RelativeSource = new RelativeSource(
                RelativeSourceMode.FindAncestor, typeof(MainWindow), 1);

            this.SetBinding(DisplayEventDetailCommandProperty, myBinding);
        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CurrentEvent = (EventModel)(sender as ListBoxItem).Content;
            DisplayEventDetailCommand.Execute(CurrentEvent);
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (lbx.SelectedItem is EventModel)
            {
                CurrentEvent = (lbx.SelectedItem as EventModel);
                DisplayEventDetailCommand.Execute(CurrentEvent);
            }
        }

        public ICommand DisplayEventDetailCommand
        {
            get { return (ICommand)this.GetValue(DisplayEventDetailCommandProperty); }
            set { this.SetValue(DisplayEventDetailCommandProperty, value); }
        }

        private EventModel CurrentEvent
        {
            get;
            set;
        }
    }
}
