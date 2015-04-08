using Helper.Controls;
using Starehe.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Starehe.Views
{
    public partial class StudentsReport : UserControl
    {
        public StudentsReport()
        {
            InitializeComponent();
            DataContextChanged += (o, e) =>
                {
                    StudentsReportVM srvm = DataContext as StudentsReportVM;
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
