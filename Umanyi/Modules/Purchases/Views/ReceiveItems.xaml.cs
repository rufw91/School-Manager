
using System.Linq;
using System.Windows.Controls;
using UmanyiSMS.Modules.Purchases.Models;
using UmanyiSMS.Modules.Purchases.ViewModels;

namespace UmanyiSMS.Modules.Purchases.Views
{
    public partial class ReceiveItems : UserControl
    {
        public ReceiveItems()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                ReceiveItemsVM rivm = this.DataContext as ReceiveItemsVM;
                if (rivm == null)
                    return;
                rivm.FindItemsAction = () =>
                {
                    FindItems f = new FindItems();
                    f.ShowDialog();
                    if (f != null)
                        if (f.SelectedItems.Count >0)
                        {
                            foreach (ItemFindModel ifm in f.SelectedItems)
                            {
                                if (rivm.NewReceipt.Items.Any(item => item.ItemID == ifm.ItemID))
                                    rivm.NewReceipt.Items.First(item => item.ItemID == ifm.ItemID).Quantity += ifm.Quantity;
                                else
                                    rivm.NewReceipt.Items.Add(new ItemPurchaseModel(ifm));
                            }
                        }
                };
            };
        }
        private void dataGrid1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            ReceiveItemsVM rivm = this.DataContext as ReceiveItemsVM;
            if ((rivm == null) || (rivm.NewReceipt == null))
                return;
            rivm.NewReceipt.RefreshOrderTotal();
        }
    }
}
