using Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Starehe.ViewModels
{
    public class ViewBooksVM: ViewModelBase
    {
        string currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
        public ViewBooksVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "VIEW BOOKS";
        }

        protected override void CreateCommands()
        {
            OpenKLBListCommand = new RelayCommand(o =>
            {
                if (OpenKLBListAction != null)
                    OpenKLBListAction.Invoke( Path.Combine(currentFolder,"klb.pdf"));
            }, o => true);
            OpenLongHornListCommand = new RelayCommand(o =>
            {
                if (OpenLongHornListAction != null)
                    OpenLongHornListAction.Invoke( Path.Combine(currentFolder,"longhorn.pdf"));
            }, o => true);
            OpenEAEPListCommand = new RelayCommand(o =>
            {
                if (OpenEAEPListAction != null)
                    OpenEAEPListAction.Invoke( Path.Combine(currentFolder,"eaep.pdf"));
            }, o => true);   
        }

        public override void Reset()
        {
         
        }
        public Action<string> OpenKLBListAction
        { get; set; }
        public Action<string> OpenLongHornListAction
        { get; set; }
        public Action<string> OpenEAEPListAction
        { get; set; }
        public ICommand OpenKLBListCommand
        { get; private set; }

        public ICommand OpenLongHornListCommand
        { get; private set; }

        public ICommand OpenEAEPListCommand
        { get; private set; }
    }
}
