
using System.Windows.Controls;
using System.Windows.Input;

namespace UmanyiSMS.Modules.Institution.Views
{
    public partial class InstitutionSetup : UserControl
    {
        public InstitutionSetup()
        {
            InitializeComponent();
            this.KeyUp += (o, e) =>
                {
                    if ((e.Key == System.Windows.Input.Key.E) && (Keyboard.Modifiers == ModifierKeys.Control))
                    {
                        txtSyncID.IsEnabled = !txtSyncID.IsEnabled;
                        txtSyncAddr.IsEnabled = !txtSyncAddr.IsEnabled;
                    }
                };
            
        }
    }
}
