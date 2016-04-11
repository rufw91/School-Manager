using System.Windows.Controls;
using System.Windows.Input;
using UmanyiSMS.ViewModels;

namespace UmanyiSMS.Views
{
    public partial class Fees : UserControl
    {
        public Fees()
        {
            InitializeComponent();
            KeyUp += (o, e) =>
            {
                if ((e.Key == Key.E) && (Keyboard.Modifiers == ModifierKeys.Control)&&(this.DataContext!=null))
                {
                    var j = DataContext as FinanceVM;
                    j.ToggleMenuVisibility();
                }
            };
        }
    }
}
