

using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.Exams.ViewModels;
using UmanyiSMS.Modules.MySystem.Views;

namespace UmanyiSMS.Modules.Exams.Views
{
    public partial class CombinedAggregateResults : UserControl
    {
        public CombinedAggregateResults()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                CombinedAggregateResultsVM vervm = DataContext as CombinedAggregateResultsVM;
                if (vervm != null)
                {
                    vervm.ShowPrintDialogAction = (p) =>
                    {
                        CustomWindow w = new CustomWindow();
                        w.MinHeight = 610;
                        w.MinWidth = 810;
                        w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        w.WindowState = WindowState.Maximized;
                        w.Content = new MyPrintDialog(p);
                        w.ShowDialog();
                    };
                }

            };
        }
    }
}
