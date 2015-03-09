using Helper.Controls;
using Starehe.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Starehe.Views
{
    /// <summary>
    /// Interaction logic for ClassResults.xaml
    /// </summary>
    public partial class AggregateResults : UserControl
    {
        public AggregateResults()
        {
            InitializeComponent();
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

                        w.Content = new PrintDialog(p);
                        w.ShowDialog();

                    };
                }
            };
        }
    }
}
