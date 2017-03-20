


using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.Exams.Controller;
using UmanyiSMS.Modules.Exams.ViewModels;
using UmanyiSMS.Modules.MySystem.Views;

namespace UmanyiSMS.Modules.Exams.Views
{
    public partial class WeightedMarkList : UserControl
    {
        public WeightedMarkList()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                WeightedMarkListVM vervm = DataContext as WeightedMarkListVM;
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
                        w.Content = new MyPrintDialog(v);
                        w.ShowDialog();
                    };
                }

            };
        }
    }
}
