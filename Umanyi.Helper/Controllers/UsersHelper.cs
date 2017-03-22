
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace UmanyiSMS.Lib.Controllers
{
    public static class UsersHelper
    {
        public static Task<bool> CreateNewUserAsync(SqlCredential cred, UserRole role, string name, byte[] photo)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                SqlConnection conn = DataAccessHelper.Helper.CreateConnection();
                try
                {
                    using (conn)
                    {
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        var login = new Login(server, cred.UserId);
                        login.LoginType = LoginType.SqlLogin;
                        login.PasswordPolicyEnforced = false;
                        login.Create(cred.Password);

                        Database db = server.Databases[Lib.Properties.Settings.Default.Info.DBName];
                        User u = new User(db, cred.UserId) { Login = login.Name };

                        u.Create();
                        u.AddToRole(role.ToString());
                        if ((role == UserRole.Principal) || (role == UserRole.SystemAdmin))
                        {
                            ServerPermissionSet sps = new ServerPermissionSet(ServerPermission.AlterAnyLogin);
                            server.Grant(sps, cred.UserId, true);
                        }
                    }
                    return true;
                }
                catch { return false; }
            });
        }

        public static Task<UserRole> GetUserRole(string userID)
        {
            return Task.Factory.StartNew<UserRole>(() =>
            {
                SqlConnection conn = DataAccessHelper.Helper.CreateConnection();
                try
                {
                    using (conn)
                    {
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);

                        if (userID.Trim().ToUpper() == "SA")
                            return UserRole.SystemAdmin;
                        
                            Database db = server.Databases[Lib.Properties.Settings.Default.Info.DBName];
                            User u = db.Users[userID];
                        if (u == null)
                            return UserRole.None;
                            var t = u.EnumRoles();
                            if (t!=null&&t.Count>0)                            
                                return (UserRole)Enum.Parse(typeof(UserRole), t[0]);                            
                    }
                    return UserRole.None;
                }
                catch { return UserRole.None; }
            });
        }

        public static Task<bool> UpdateUserAsync(SqlCredential credential, UserRole userRole)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                SqlConnection conn = DataAccessHelper.Helper.CreateConnection();
                try
                {
                    using (conn)
                    {
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        var login = server.Logins[credential.UserId];

                        string pwd;

                        IntPtr ptr = Marshal.SecureStringToBSTR(credential.Password);
                        pwd = Marshal.PtrToStringBSTR(ptr);

                        login.ChangePassword(pwd);
                        login.Alter();
                        Marshal.FreeBSTR(ptr);

                        if (credential.UserId.Trim().ToUpper() != "SA")
                        {
                            Database db = server.Databases[Lib.Properties.Settings.Default.Info.DBName];
                            User u = db.Users[credential.UserId];
                            foreach (DatabaseRole dbr in db.Roles)
                            {
                                if (!dbr.IsFixedRole)
                                    if (u.IsMember(dbr.Name))
                                        dbr.DropMember(u.Name);
                            }
                            u.AddToRole(userRole.ToString());
                            u.Alter();                            
                        }
                    }
                    return true;
                }
                catch { return false; }
            });
        }

        public static Task<bool> RemoveUserAsync(string userID)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                SqlConnection conn = DataAccessHelper.Helper.CreateConnection();
                try
                {
                    using (conn)
                    {
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        var login = server.Logins[userID];
                        login.Drop();

                        Database db = server.Databases[Lib.Properties.Settings.Default.Info.DBName];
                        User u = db.Users[userID];

                        u.Drop();
                        
                    }
                    return true;
                }
                catch { return false; }
            });
        }
        
    }
}
