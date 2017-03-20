


using UmanyiSMS.Modules.Exams.ViewModels;
using UmanyiSMS.Lib.Controls;
using System.Windows;
using System.Windows.Controls;

namespace UmanyiSMS.Modules.Exams.Views
{
    public partial class AggregateResults : UserControl
    {
        public AggregateResults()
        {
            InitializeComponent();
            viewer.FitToWidth();
            this.DataContextChanged += (o, e) =>
            {
                if (DataContext != null)
                {
                    AggregateResultsVM nfpvm = DataContext as AggregateResultsVM;
                    nfpvm.ShowPrintDialogAction = (p) =>
                    {
                        CustomWindow w = new CustomWindow();
                        w.MinHeight = 610;
                        w.MinWidth = 810;
                        w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        w.WindowState = WindowState.Maximized;

                        w.Content = new MySystem.Views.MyPrintDialog(p);
                        w.ShowDialog();

                    };
                }
            };
        }
    }
}
