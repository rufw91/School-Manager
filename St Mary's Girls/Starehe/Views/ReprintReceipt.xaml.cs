using Helper.Controls;
using Starehe.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Starehe.Views
{
    /// <summary>
    /// Interaction logic for ReprintReceipt.xaml
    /// </summary>
    public partial class ReprintReceipt : UserControl
    {
        public ReprintReceipt()
        {
            InitializeComponent();
            viewer.FitToWidth();

        }
    }
}
