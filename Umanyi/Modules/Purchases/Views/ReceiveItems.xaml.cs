

using System.Windows.Controls;
using UmanyiSMS.Modules.Purchases.Models;
using UmanyiSMS.Modules.Purchases.ViewModels;

namespace UmanyiSMS.Views
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
                        if (f.SelectedItems != null)
                        {
                            foreach (ItemFindModel ifm in f.SelectedItems)
                                rivm.NewReceipt.Items.Add(new ItemPurchaseModel(ifm));
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
