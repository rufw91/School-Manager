using Helper.Models;
using UmanyiSMS.ViewModels;
using System.Windows.Controls;

namespace UmanyiSMS.Views
{
    public partial class StockTaking : UserControl
    {
        public StockTaking()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                StockTakingVM rivm = this.DataContext as StockTakingVM;
                if (rivm == null)
                    return;
                rivm.FindItemsAction = () =>
                {
                    FindItems f = new FindItems();
                    f.ShowDialog();
                    if (f != null)
                        if (f.SelectedItems != null)
                        {
                            foreach (ItemFindModel ifm in f.SelectedItems)
                                rivm.NewStockTaking.Items.Add(new ItemStockTakingModel(ifm));
                        }
                };
            };
        }
    }
}
