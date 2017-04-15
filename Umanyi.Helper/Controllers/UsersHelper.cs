
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
                        if ((role == UserRole.Principal) || (role == UserRole.SystemAdmin))
                        {
                            ServerPermissionSet sps = new ServerPermissionSet(ServerPermission.AlterAnyLogin);
                            server.Grant(sps, cred.UserId, true);
                            if (role == UserRole.SystemAdmin)
                                login.AddToRole("sysadmin");
                            else
                                u.AddToRole(role.ToString());
                        }
                        else
                            u.AddToRole(role.ToString());
                    }
                    return true;
                }
                catch { return false; }
            });
        }
                
        internal static Task<List<UserRole>> GetUserRolesAsync(string userId)
        {
            return Task.Factory.StartNew<List<UserRole>>(() =>

            {
                if (userId.ToUpper() == "SA")
                    return GetAllRoles();

                else
                {
                    SqlConnection conn = DataAccessHelper.Helper.CreateConnection();

                    List<UserRole> temp = new List<UserRole>();
                    try
                    {
                        using (conn)
                        {
                            var sc = new ServerConnection(conn);
                            Server server = new Server(sc);
                            Database db = server.Databases["UmanyiSMS"];

                            var u = db.Users[userId];
                            if (u == null)
                            {
                                temp.Add(UserRole.None);
                                return temp;
                            }


                            StringCollection coll = u.EnumRoles();
                            for (int i = 0; i < coll.Count; i++)
                                temp.Add((UserRole)Enum.Parse(typeof(UserRole), coll[i]));

                            if ((temp.Count == 0) && (new ServerRole(server, "sysadmin").EnumMemberNames().Contains(userId)))
                                return GetAllRoles();
                            else
                                temp = GetChildRoles((UserRole)Enum.Parse(typeof(UserRole),temp[0].ToString()));
                        }
                    }
                    catch { }

                    return temp;
                }
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

        public static List<UserRole> GetChildRoles(UserRole role)
        {
            List<UserRole> t = new List<UserRole>();
            var arr = Enum.GetValues(typeof(UserRole));
            foreach (var i in arr)
                if (role >= (UserRole)i)
                    t.Add((UserRole)i);
            return t;

        }

        public static List<UserRole> GetAllRoles()
        {
            List<UserRole> t = new List<UserRole>();
            var arr = Enum.GetValues(typeof(UserRole));
            foreach (var i in arr)
                    t.Add((UserRole)i);
            return t;

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
