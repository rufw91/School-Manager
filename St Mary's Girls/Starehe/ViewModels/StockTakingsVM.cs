using Helper;
using System.Security.Permissions;

namespace Starehe.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class StockTakingsVM:ViewModelBase
    {
        public StockTakingsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "Stock Taking";
        }

        protected override void CreateCommands()
        {
            
        }

        public override void Reset()
        {
           
        }
    }
}
