using System.Security.Permissions;
using System.Windows.Interop;
using System.Windows.Media;
using UmanyiSMS.Lib;

namespace UmanyiSMS.Modules.System.ViewModels
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
        
        public override void Reset()
        {
           
        }
    }
}
