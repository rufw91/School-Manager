using Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class OnlineResourcesVM:ViewModelBase
    {
        public OnlineResourcesVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "ONLINE RESOUCES";
        }

        protected override void CreateCommands()
        {
            OpenLink1Command = new RelayCommand(o =>
            {
                try
                {
                    Process.Start("https://msdn.microsoft.com/en-us/library/dd239338(v=sql.105).aspx");
                }
                catch { }
            }, o => IsActive);
            OpenLink2Command = new RelayCommand(o =>
            {
                Process.Start("http://www.mssqltips.com/sqlservertutorial/2100/report-builder-30/");
            }, o => IsActive);
            OpenLink3Command = new RelayCommand(o =>
            {
                Process.Start("https://www.google.com/search?q=Report+Builder+3.0+tutorial&ie=utf-8&oe=utf-8");
            }, o => IsActive);
        }
        public ICommand OpenLink1Command
        {
            get;
            private set;
        }
        public ICommand OpenLink2Command
        {
            get;
            private set;
        }
        public ICommand OpenLink3Command
        {
            get;
            private set;
        }
        public override void Reset()
        {
           
        }
    }
}
