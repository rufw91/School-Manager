using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Helper
{
    public static class LoginHelper
    {        
        public async static Task<bool> AuthenticateUser(SqlCredential cred)
        {
            try
            {
                bool test=
                DataAccessHelper.TestCredential(cred);
                if (test)
                {
                    DataAccessHelper.SetCredential(cred);
                    string[] roles = await GetUserRolesAsync(cred.UserId);
                    
                    if ((roles.Length == 1) && (roles[0] == UserRole.None.ToString()))
                        return false;
                    SetPrincipal(cred.UserId, roles);
                }
                return test;
            }
            catch{ return false; }
        }

        private static string[] GetAllRoles()
        {
            return Enum.GetNames(typeof(UserRole));
        }

        private static Task<string[]> GetUserRolesAsync(string userId)
        {            
            return Task.Run<string[]>(() =>
            {
                if (userId.ToUpper() == "SA")
                    return GetAllRoles();

                else
                {
                    SqlConnection conn = DataAccessHelper.CreateConnection();
                    string maxRole=UserRole.None.ToString();
                    string[] temp = new string[1];
                    temp[0] = "None";
                    try
                    {
                        using (conn)
                        {
                            var sc = new ServerConnection(conn);
                            Server server = new Server(sc);
                            Database db = server.Databases["Starehe"];

                            StringCollection coll=db.Users[userId].EnumRoles();
                            temp = new string[coll.Count];
                            for (int i = 0; i < coll.Count; i++)
                                temp[i] = coll[i];
                        }
                    }
                    catch { }

                    return temp;
                }                        
            });
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
