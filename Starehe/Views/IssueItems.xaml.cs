using Helper.Models;
using UmanyiSMS.ViewModels;
using System.Windows.Controls;

namespace UmanyiSMS.Views
{
    public partial class IssueItems : UserControl
    {
        public IssueItems()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                IssueItemsVM iivm = this.DataContext as IssueItemsVM;
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
                                iivm.NewIssue.Items.Add(new ItemIssueModel(ifm));
                        }
                };
            };
        }
    }
}
