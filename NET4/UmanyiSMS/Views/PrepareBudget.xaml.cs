using Helper.Models;
using System.Windows.Controls;
using System.Linq;
using UmanyiSMS.ViewModels;
using Helper;
using System.Collections.Generic;
using System.Windows.Data;

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

                iivm.FindAccountsAction = () =>
                {
                    FindObject f = new FindObject();
                    f.SearchProperty = "Description";
                    f.Title = "Find Accounts(s)";
                    f.GetItemsFunction = () =>
                        {
                            return DataAccess.GetAllItemCategoriesAsync().Result;
                        };
                    DataGridTextColumn col1 = new DataGridTextColumn();
                    col1.Header = "Account ID";
                    col1.Binding = new Binding("ItemCategoryID");
                    col1.Width = new DataGridLength(200, DataGridLengthUnitType.Auto);
                    DataGridTextColumn col2 = new DataGridTextColumn();
                    col2.Header = "Name";
                    col2.Binding = new Binding("Description");
                    col2.Width = new DataGridLength(200, DataGridLengthUnitType.Auto);
                    f.DisplayColumns = new List<DataGridTextColumn>() { col1,col2};
                    f.ShowDialog();
                    if (f != null)
                        if (f.SelectedItems != null)
                        {
                            foreach (ItemCategoryModel ifm in f.SelectedItems)
                            {
                                if (!iivm.NewBudget.Accounts.Any(o1 => (o1.Name == ifm.Description) && (o1.AccountID == ifm.ItemCategoryID)))
                                    iivm.NewBudget.Accounts.Add(new BudgetAccountModel(ifm));
                            }
                        }
                };
            };
        }
    }
}
