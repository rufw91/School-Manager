using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Interop;
using System.Windows.Media;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Deputy")]
    public class GeneralSettingsVM : ViewModelBase
    {
        private bool useHardwareRendering;
        public GeneralSettingsVM()
            : base()
        {
            InitVars();
            CreateCommands();
        }
        protected override void CreateCommands()
        {
            
        }

        protected override void InitVars()
        {            
            Title = "GENERAL";
            UseHardwareRendering = RenderOptions.ProcessRenderMode == RenderMode.Default;
        }

        public bool UseHardwareRendering
        {
            get { return useHardwareRendering; }
            set
            {
                if (value != this.useHardwareRendering)
                {
                    this.useHardwareRendering = value;
                    if (!useHardwareRendering)
                        RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
                    else
                        RenderOptions.ProcessRenderMode = RenderMode.Default;
                    NotifyPropertyChanged("UseHardwareRendering");
                }
            }
        }

        public bool AutomaticBackup
        {
            get { return Helper.Properties.Settings.Default.AutomaticBackup; }
            set
            {
                if (value != Helper.Properties.Settings.Default.AutomaticBackup)
                {
                    Helper.Properties.Settings.Default.AutomaticBackup = value;
                    Helper.Properties.Settings.Default.Save();
                    NotifyPropertyChanged("AutomaticBackup");
                }
            }
        }

        public override void Reset()
        {
           
        }
    }
}
