using Helper;
using Helper.Controls;
using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.ViewModels;

namespace UmanyiSMS.Views
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
                        w.Content = new PrintDialog(p);
                        w.ShowDialog();
                    };
                }

            };
        }
    }
}
