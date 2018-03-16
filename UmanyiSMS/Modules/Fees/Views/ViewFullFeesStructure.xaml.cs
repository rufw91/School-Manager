
using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.Fees.ViewModels;
using UmanyiSMS.Modules.MySystem.Views;

namespace UmanyiSMS.Modules.Fees.Views
{
    public partial class ViewFullFeesStructure : UserControl
    {
        public ViewFullFeesStructure()
        {
            InitializeComponent();
            viewer.FitToWidth();

            DataContextChanged += (o, e) =>
            {
                ViewFullFeesStructureVM arvm = DataContext as ViewFullFeesStructureVM;
                if (arvm == null)
                    return;
                arvm.ShowFullPreviewAction = (d) =>
                {
                    CustomWindow w = new CustomWindow();
                    w.MinHeight = 610;
                    w.MinWidth = 810;
                    w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    w.WindowState = WindowState.Maximized;

                    w.Content = new MyPrintDialog(d);
                    w.ShowDialog();
                };
            };
        }
    }
}
