
using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.Library.ViewModels;
using UmanyiSMS.Modules.MySystem.Views;

namespace UmanyiSMS.Modules.Library.Views
{
    /// <summary>
    /// Interaction logic for AllUnreturnedBooks.xaml
    /// </summary>
    public partial class AllUnreturnedBooks : UserControl
    {
        public AllUnreturnedBooks()
        {
            InitializeComponent();
            DataContextChanged += (o, e) =>
            {
                AllUnreturnedBooksVM srvm = DataContext as AllUnreturnedBooksVM;
                if (srvm == null)
                    return;
                srvm.ShowFullPreviewAction = (d) =>
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
