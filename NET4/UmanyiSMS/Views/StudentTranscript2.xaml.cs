using Helper.Controls;
using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.ViewModels;

namespace UmanyiSMS.Views
{
    public partial class StudentTranscript2 : UserControl
    {
        public StudentTranscript2()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                if (DataContext != null)
                {
                    StudentTranscriptVM nfpvm = DataContext as StudentTranscriptVM;
                    nfpvm.ShowPrintDialogAction = (p) =>
                    {
                        CustomWindow w = new CustomWindow();
                        w.MinHeight = 610;
                        w.MinWidth = 810;
                        w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        w.WindowState = WindowState.Maximized;

                        w.Content = new PrintDialog(p);
                        w.ShowDialog();

                    };
                }
            };
        }
    }
}
