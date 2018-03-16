
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace UmanyiSMS.Modules.MySystem.Views
{
    public partial class MyPrintDialog : UserControl
    {
        FixedDocument viewObject;
        public MyPrintDialog(FixedDocument printObject)
        {
            viewObject = printObject;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                docViewer.Document = viewObject;
            }
            catch { }
        }
    }
}
