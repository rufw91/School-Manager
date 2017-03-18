using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.Lib.Controls;
using UmanyiSMS.Modules.Exams.Controller;
using UmanyiSMS.Modules.Exams.ViewModels;

namespace UmanyiSMS.Views
{
    /// <summary>
    /// Interaction logic for MarkLists.xaml
    /// </summary>
    public partial class MarkLists : UserControl
    {
        public MarkLists()
        {
            InitializeComponent();
            this.DataContextChanged += (o, e) =>
            {
                MarkListsVM vervm = DataContext as MarkListsVM;
                if (vervm != null)
                {
                    vervm.ShowStudentTranscriptAction = (p) =>
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
                    vervm.ShowClassStudentsTranscriptAction = (p) =>
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
