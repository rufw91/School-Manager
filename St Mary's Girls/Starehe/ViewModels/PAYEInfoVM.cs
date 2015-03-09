using Helper;
using Helper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class PAYEInfoVM: ViewModelBase
    {
        string currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
        public PAYEInfoVM()
        {
         InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "P.A.Y.E. INFO";
        }

        protected override void CreateCommands()
        {
            OpenKRAGuideCommand = new RelayCommand(o =>
            {
                if (OpenKRAGuideAction != null)
                    OpenKRAGuideAction.Invoke(Path.Combine(currentFolder, "PAYEGuide2009.pdf"));
            }, o => true);
        }

        public ICommand OpenKRAGuideCommand
        { get; private set; }

        public Action<string> OpenKRAGuideAction
        { get; set; }

        public override void Reset()
        {
            
        }
    }
}
