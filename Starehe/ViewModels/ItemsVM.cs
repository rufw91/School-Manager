using Helper;
using System;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "Accounts")]
    public class ItemsVM:ViewModelBase
    {
        public ItemsVM()
        {
            InitVars();
            CreateCommands();
        }
        protected override void InitVars()
        {
            Title = "Items";
        }

        protected override void CreateCommands()
        {
           
        }

        public override void Reset()
        {
        }
    }
}
