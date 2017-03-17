using System.Security.Permissions;
using System.Windows;
using System.Windows.Input;
using UmanyiSMS.Lib;
using UmanyiSMS.Lib.Presentation;

namespace UmanyiSMS.Modules.System.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "None")]
    public class HomePageVM: ViewModelBase
    {
        private Window window;
        public HomePageVM()
        {
            CreateCommands();
            
        }

        protected override void InitVars()
        {
        }

        protected override void CreateCommands()
        {
            LogoutCommand = new RelayCommand(o =>
            {
                if (MessageBoxResult.Yes== MessageBox.Show("Are you sure you would like to logout?","Info", 
                    MessageBoxButton.YesNo, MessageBoxImage.Information))
                {
                    App.Restart();
                }

            }, o => true);
        }

        public ICommand LogoutCommand
        {
            get;
            private set;
        }
        public override void Reset()
        {
            
        }

        internal void SetWindow(Window window)
        {
            this.window = window;
        }
    }
}
