using UmanyiSMS.ViewModels;
using System.Windows.Controls;

namespace UmanyiSMS.Views
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
