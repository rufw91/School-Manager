
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Threading.Tasks;

namespace UmanyiSMS.Lib.Controllers
{
    public static class LoginHelper
    {        
        public async static Task<bool> AuthenticateUser(SqlCredential cred)
        {
            try
            {
                bool test=
                DataAccessHelper.Helper.TestCredential(cred);
                if (test)
                {
                   DataAccessHelper.Helper.SetCredential(cred);
                    string[] roles = await GetUserRolesAsync(cred.UserId);
                    
                    SetPrincipal(cred.UserId, roles);
                }
                return test;
            }
            catch{ return false; }
        }

        private async static Task<string[]> GetUserRolesAsync(string userId)
        {
            var rs = await UsersHelper.GetUserRolesAsync(userId);
            List<string> t = new List<string>();
            foreach (var y in rs)
                t.Add(y.ToString());
            return t.ToArray();
        }

        private static void SetPrincipal(string userId,string[] roles)
        {
            GenericIdentity MyIdentity = new GenericIdentity(userId);
            GenericPrincipal MyPrincipal =
                new GenericPrincipal(MyIdentity, roles);
            AppDomain.CurrentDomain.SetThreadPrincipal(MyPrincipal);
        }
    }
}
