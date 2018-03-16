using System.Windows;
using System.Windows.Controls;
using UmanyiSMS.Modules.MySystem.ViewModels;

namespace UmanyiSMS.Modules.MySystem.Views
{
    /// <summary>
    /// Interaction logic for LogWindow.xaml
    /// </summary>
    public partial class LogWindow : UserControl
    {
        public LogWindow()
        {
            InitializeComponent();
            DataContextChanged += (o, e) =>
              {
                  if (DataContext!=null)
                  {
                      LogWindowVM lwvm = DataContext as LogWindowVM;
                      lwvm.CloseAction = () =>
                      {
                          Window.GetWindow(this).Close();
                      };
                  }
              };
        }
    }
}
