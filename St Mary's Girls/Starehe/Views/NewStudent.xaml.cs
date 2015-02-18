using Starehe.ViewModels;
using System.Windows.Controls;

namespace Starehe.Views
{
    public partial class NewStudent : UserControl
    {
        public NewStudent()
        {
            InitializeComponent();
            DataContextChanged += (o, e) =>
                {
                    NewStudentVM nsvm = DataContext as NewStudentVM;
                    if (nsvm != null)
                        nsvm.ShowImportWindowAction = () =>
                            {
                                ImportWizardMainWindow im = new ImportWizardMainWindow();
                                im.ShowDialog();
                            };
                };
        }
    }
}
