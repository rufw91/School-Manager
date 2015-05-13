using Helper.Controls;
using UmanyiSMS.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace UmanyiSMS.Views
{
    public partial class PriceLists : UserControl
    {
        public PriceLists()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                if (DataContext != null)
                {
                    PriceListsVM nfpvm = DataContext as PriceListsVM;
                    nfpvm.OpenListAction = (p) =>
                    {
                        CustomWindow w = new CustomWindow();
                        w.MinHeight = 610;
                        w.MinWidth = 810;
                        w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        w.WindowState = WindowState.Maximized;

                        w.Content = new PDFViewVM(p);
                        w.ShowDialog();
                    };
                }
            };
        }

    }
}

