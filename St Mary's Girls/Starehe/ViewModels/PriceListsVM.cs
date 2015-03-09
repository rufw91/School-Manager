using Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class PriceListsVM: ViewModelBase
    {
        string currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
        public PriceListsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "PRICE LISTS";
        }

        protected override void CreateCommands()
        {
            OpenKLBListCommand = new RelayCommand(o =>
            {
                if (OpenListAction != null)
                    OpenListAction.Invoke(Path.Combine(currentFolder, "klb.pdf"));
            }, o => true);
            OpenLongHornListCommand = new RelayCommand(o =>
            {
                if (OpenListAction != null)
                    OpenListAction.Invoke(Path.Combine(currentFolder, "longhorn.pdf"));
            }, o => true);
            OpenEAEPListCommand = new RelayCommand(o =>
            {
                if (OpenListAction != null)
                    OpenListAction.Invoke(Path.Combine(currentFolder, "eaep.pdf"));
            }, o => true);
            OpenJKFListCommand = new RelayCommand(o =>
            {
                if (OpenListAction != null)
                    OpenListAction.Invoke(Path.Combine(currentFolder, "jkf.pdf"));
            }, o => true);   
        }

        public override void Reset()
        {
         
        }
        public Action<string> OpenListAction
        { get; set; }
        public ICommand OpenKLBListCommand
        { get; private set; }

        public ICommand OpenLongHornListCommand
        { get; private set; }

        public ICommand OpenEAEPListCommand
        { get; private set; }

        public ICommand OpenJKFListCommand
        { get; private set; }
    }
}
