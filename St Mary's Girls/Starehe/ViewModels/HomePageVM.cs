using Helper;
using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "None")]
    public class HomePageVM: ViewModelBase
    {
        public HomePageVM()
        {
            CreateCommands();
            
        }

        protected override void InitVars()
        {
        }

        protected override void CreateCommands()
        {
            NavigatePageCommand = new RelayCommand(o =>
            {
                
            },o=>true);
        }

        public ICommand NavigatePageCommand
        {
            get;
            private set;
        }
        public override void Reset()
        {
            
        }
        
    }
}
