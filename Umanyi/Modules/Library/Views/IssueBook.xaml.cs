
using System.Windows.Controls;
using UmanyiSMS.Modules.Library.Models;
using UmanyiSMS.Modules.Library.ViewModels;

namespace UmanyiSMS.Views
{
    public partial class IssueBook : UserControl
    {
        public IssueBook()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                IssueBookVM iivm = this.DataContext as IssueBookVM;
                if (iivm == null)
                    return;
                iivm.FindBooksAction = () =>
                {
                    FindBooks f = new FindBooks();
                    f.ShowDialog();
                    if (f != null)
                        if (f.SelectedItems != null)
                        {
                            foreach (BookModel ifm in f.SelectedItems)
                                iivm.ThisIssue.Add(new BookModel(ifm));
                        }
                };
            };
        }

    }
}
