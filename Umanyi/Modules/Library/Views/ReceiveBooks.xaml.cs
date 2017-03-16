using Helper.Models;
using System.Windows.Controls;
using UmanyiSMS.ViewModels;

namespace UmanyiSMS.Views
{
    /// <summary>
    /// Interaction logic for ReceiveBooks.xaml
    /// </summary>
    public partial class ReceiveBooks : UserControl
    {
        public ReceiveBooks()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                ReceiveBooksVM rivm = this.DataContext as ReceiveBooksVM;
                if (rivm == null)
                    return;
                rivm.FindBooksAction = () =>
                {
                    FindBooks f = new FindBooks();
                    f.ShowDialog();
                    if (f != null)
                        if (f.SelectedItems != null)
                        {
                            foreach (BookModel ifm in f.SelectedItems)
                                rivm.NewReceipt.Items.Add(new BookReceiptModel(ifm));
                        }
                };
            };
        }
        private void dataGrid1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            ReceiveBooksVM rivm = this.DataContext as ReceiveBooksVM;
            if ((rivm == null) || (rivm.NewReceipt == null))
                return;
            rivm.NewReceipt.RefreshOrderTotal();
        }
    }
}
