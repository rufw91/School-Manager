using Helper;
using Helper.Controls;
using Starehe.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Starehe.Views
{
    /// <summary>
    /// Interaction logic for CombinedMarkList.xaml
    /// </summary>
    public partial class CombinedMarkLists : UserControl
    {
        public CombinedMarkLists()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                CombinedMarkListsVM vervm = DataContext as CombinedMarkListsVM;
                if (vervm != null)
                {
                    vervm.ShowClassTranscriptAction = (p) =>
                    {
                        CustomWindow w = new CustomWindow();
                        w.MinHeight = 610;
                        w.MinWidth = 810;
                        w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        w.WindowState = WindowState.Maximized;
                        var v = DocumentHelper.GenerateDocument(p);
                        w.Content = new PrintDialog(v);
                        w.ShowDialog();
                    };
                }

            };
        }
    }
}
