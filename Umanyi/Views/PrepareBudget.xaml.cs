using Helper.Models;
using System.Windows.Controls;
using System.Linq;
using UmanyiSMS.ViewModels;

namespace UmanyiSMS.Views
{
    public partial class PrepareBudget : UserControl
    {
        public PrepareBudget()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                PrepareBudgetVM iivm = this.DataContext as PrepareBudgetVM;
                if (iivm == null)
                    return;
                iivm.FindItemsAction = () =>
                {
                    FindItems f = new FindItems();
                    f.ShowDialog();
                    if (f != null)
                        if (f.SelectedItems != null)
                        {
                            foreach (ItemFindModel ifm in f.SelectedItems)
                            {
                                if (!iivm.NewBudget.Entries.Any(o1 => (o1.Description == ifm.Description) && (o1.AccountID == ifm.ItemCategoryID)))
                                    iivm.NewBudget.Entries.Add(new BudgetEntryModel(ifm));
                            }
                        }
                };
            };
        }
    }
}
