using Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Teacher")]
    public class LocalTutorialsVM : ViewModelBase
    {
        public LocalTutorialsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "TUTORIALS";
        }

        protected override void CreateCommands()
        {
            OpenTut1Command = new RelayCommand(o =>
            {
                if (OpenPDFViewer != null)
                    OpenPDFViewer.Invoke(Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\Tut1.pdf");
            }, o => true);
            OpenTut2Command = new RelayCommand(o =>
            {
                if (OpenPDFViewer != null)
                    OpenPDFViewer.Invoke(Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\Tut2.pdf");
            }, o => true);
            OpenTut3Command = new RelayCommand(o =>
            {
                if (OpenPDFViewer != null)
                    OpenPDFViewer.Invoke(Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\Tut3.pdf");
            }, o => true);
            OpenInDefaultPlayerCommand = new RelayCommand(o =>
            {
                try
                {
                    Process.Start(Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\Video1.mp4");
                    Process.Start(Path.GetDirectoryName(Application.ResourceAssembly.Location) + @"\Video2.mp4");
                }
                catch { }
            }, o => IsActive);
        }

        public Action<string> OpenPDFViewer
        {
            get;
            set;
        }
        public ICommand OpenInDefaultPlayerCommand
        {
            get;
            private set;
        }
        public ICommand OpenTut1Command
        {
            get;
            private set;
        }
        public ICommand OpenTut2Command
        {
            get;
            private set;
        }
        public ICommand OpenTut3Command
        {
            get;
            private set;
        }
        public override void Reset()
        {
           
        }
    }
}
