using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.MySystem.ViewModels;

namespace UmanyiSMS.Modules.MySystem.Views
{
    public partial class SetupWizard : CustomWindow
    {
        public SetupWizard()
        {
            InitializeComponent();
            DataContext = new SetupWizardVM();
        }
    }
}
