using Helper.Controls;
using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.ViewModels;

namespace UmanyiSMS.Views
{
    public partial class EventsReport : UserControl
    {
        public EventsReport()
        {
            InitializeComponent();
            DataContextChanged += (o, e) =>
            {
                EventsReportVM srvm = DataContext as EventsReportVM;
                if (srvm == null)
                    return;
                srvm.ShowFullPreviewAction = (d) =>
                {
                    CustomWindow w = new CustomWindow();
                    w.MinHeight = 610;
                    w.MinWidth = 810;
                    w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    w.WindowState = WindowState.Maximized;

                    w.Content = new PrintDialog(d);
                    w.ShowDialog();
                };
            };
        }
    }
}
