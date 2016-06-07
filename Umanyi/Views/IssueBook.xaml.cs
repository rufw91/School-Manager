using Helper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UmanyiSMS.ViewModels;

namespace UmanyiSMS.Views
{
    /// <summary>
    /// Interaction logic for IssueBook.xaml
    /// </summary>
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
