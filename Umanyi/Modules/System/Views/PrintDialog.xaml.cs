
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace UmanyiSMS
{
    public partial class PrintDialog : UserControl
    {
        FixedDocument viewObject;
        public PrintDialog(FixedDocument printObject)
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
