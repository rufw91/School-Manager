using Helper.Models;
using Helper.Security;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace Helper
{
    public static class UsersHelper
    {
        public static Task<bool> CreateNewUserAsync(SqlCredential cred, UserRole role, string name, byte[] photo)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                SqlConnection conn = DataAccessHelper.CreateConnection();
                try
                {
                    bool succ;
                    using (conn)
                    {
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        var login = new Login(server, cred.UserId);
                        login.LoginType = LoginType.SqlLogin;
                        login.PasswordPolicyEnforced = false;
                        login.Create(cred.Password);

                        Database db = server.Databases[Helper.Properties.Settings.Default.DefaultDBName];
                        User u = new User(db, cred.UserId) { Login = login.Name };

                        u.Create();
                        List<UserRole> roles = GetChildRoles(role);
                        foreach (UserRole r in roles)
                            u.AddToRole(r.ToString());
                        succ = SaveUserInfo(cred, role, name, photo).Result;

                        if ((role == UserRole.Principal) || (role == UserRole.SystemAdmin))
                        {
                            ServerPermissionSet sps = new ServerPermissionSet(ServerPermission.AlterAnyLogin);
                            server.Grant(sps, cred.UserId,true);
                        }
                    }
                    return true;
                }
                catch { return false; }
            });
        }

        private static List<UserRole> GetChildRoles(UserRole role)
        {
            List<UserRole> temp = new List<UserRole>() { UserRole.None };
            switch(role)
            {
                case UserRole.None: 
                    break;
                case UserRole.User: 
                    temp.Add(UserRole.User); 
                    break;
                case UserRole.Teacher: 
                    temp.Add(UserRole.User); 
                    temp.Add(UserRole.Teacher);
                    break;
                case UserRole.Accounts: 
                    temp.Add(UserRole.User); 
                    temp.Add(UserRole.Accounts); 
                    break;
                case UserRole.Deputy: 
                    temp.Add(UserRole.User);                     
                    temp.Add(UserRole.Teacher); 
                    temp.Add(UserRole.Accounts);
                    temp.Add(UserRole.Deputy);
                    break;
                case UserRole.Principal: 
                    temp.Add(UserRole.User);
                    temp.Add(UserRole.Teacher);
                    temp.Add(UserRole.Accounts);
                    temp.Add(UserRole.Deputy);
                    temp.Add(UserRole.Principal);
                    break;
                case UserRole.SystemAdmin: 
                    temp.Add(UserRole.User); 
                    temp.Add(UserRole.Teacher);
                    temp.Add(UserRole.Accounts); 
                    temp.Add(UserRole.Deputy);
                    temp.Add(UserRole.Principal);
                    temp.Add(UserRole.SystemAdmin);
                    break;
            }
            return temp;
        }

        private static Task<bool> SaveUserInfo(SqlCredential cred, UserRole role, string name, byte[] photo)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\n" +
                    " INSERT INTO [Users].[User] (UserID,Name,SPhoto) " +
                   "VALUES('" + cred.UserId + "','" + name + "',@sPhoto)";
                Array allRoles = Enum.GetValues(typeof(UserRole));
                for (int i = 0; i < allRoles.Length; i++)
                {
                    if (role >= (UserRole)allRoles.GetValue(i))
                        insertStr += "\r\nINSERT INTO [Users].[UserDetail] (UserID,UserRoleID) " +
                            "VALUES('" + cred.UserId + "'," + i + ")";
                }
                insertStr += "\r\nCOMMIT";
                DataAccessHelper.ExecuteNonQueryWithParameters(insertStr, 
                    new ObservableCollection<SqlParameter>() { new SqlParameter("@sPhoto", photo) });
                return true;
            });
        }

        private static Task<bool> RemoveUserInfo(string userID)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\nDELETE FROM [Users].[UserDetail] WHERE UserID='" + userID + "'\r\n";

                insertStr += "DELETE FROM [Users].[User] WHERE UserID='" + userID + "'\r\nCOMMIT";
                DataAccessHelper.ExecuteNonQuery(insertStr);
                return true;
            });
        }

        private static Task<bool> UpdateUserInfo(SqlCredential cred, UserRole role)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                string insertStr = "BEGIN TRANSACTION\r\nDELETE FROM [Users].[UserDetail] WHERE UserID='" + cred.UserId + "'\r\n";

                Array allRoles = Enum.GetValues(typeof(UserRole));
                for (int i = 0; i < allRoles.Length; i++)
                {
                    if (role >= (UserRole)allRoles.GetValue(i))
                        insertStr += "\r\nINSERT INTO [Users].[UserDetail] (UserID,UserRoleID) " +
                                "VALUES('" + cred.UserId + "'," + i + ")";
                }

                insertStr += "\r\nCOMMIT";
                DataAccessHelper.ExecuteNonQuery(insertStr);
                return true;
            });
        }

        public static Task<bool> UserExists(string p)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                SqlConnection conn = DataAccessHelper.CreateConnection();
                try
                {
                    using (conn)
                    {
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        foreach (Login l in server.Logins)
                        {
                            if (l.Name.ToUpperInvariant() == p.ToUpperInvariant())
                                return true;
                        }
                        Database db = new Database(server, Helper.Properties.Settings.Default.DefaultDBName);
                        foreach (User u in db.Users)
                        {
                            if (u.Name.ToUpperInvariant() == p.ToUpperInvariant())
                                return true;
                        }
                    }
                }
                catch { }
                return false;
            });
        }

        public static UserModel CurrentUser
        { get { return GetCurrUser(); } }

        public static int CurrentUserId
        {
            get
            {
                UserModel u = new UserModel();
                IIdentity ii = Thread.CurrentPrincipal.Identity; int i; int.TryParse(ii.Name, out i); return i;
            }
        }

        static UserModel GetCurrUser()
        {
            UserModel u = new UserModel();
            IIdentity ii = Thread.CurrentPrincipal.Identity;
            int i; 
            if (int.TryParse(ii.Name, out i))
                u = GetCurrDbUser(i);
            else
            {
                u.UserName = ii.Name;
                u.Photo = null;
                u.UserID = ii.Name;
                u.Role = "SystemAdmin";
            }
            return u;
        }
        static UserModel GetCurrDbUser(int userID)
        {
            UserModel temp = new UserModel();
            string selectStr = "SELECT UserID,Name,SPhoto FROM [Users].[User] WHERE UserID=" + userID;
            DataTable dt=DataAccessHelper.ExecuteNonQueryWithResultTable(selectStr);
            if (dt.Rows.Count>0)
            {
                temp.UserID = dt.Rows[0][0].ToString();
                temp.UserName = dt.Rows[0][1].ToString();
                temp.Role = GetUserRole(userID).ToString();
                temp.Photo = (byte[])dt.Rows[0][2]; ;
            }
            return temp;
        }
        public static UserRole GetUserRole(int userID)
        {
            string selectStr = "SELECT UserRoleID FROM [Users].[UserDetail] WHERE UserID='" + userID + "'";
            ObservableCollection<string> coll = DataAccessHelper.CopyFromDBtoObservableCollection(selectStr);
            List<UserRole> roles = new List<UserRole>();
            foreach (string s in coll)
                roles.Add((UserRole)int.Parse(s));
            UserRole maxRole = UserRole.None;
            foreach (UserRole ur in roles)
                maxRole = (UserRole)Math.Max((int)maxRole, (int)ur);
            return maxRole;
        }

        public static Task<bool> UpdateUserAsync(SqlCredential credential, UserRole userRole)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                SqlConnection conn = DataAccessHelper.CreateConnection();
                try
                {
                    bool succ;
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
                        succ = true;
                        if (credential.UserId.Trim().ToUpper() != "SA")
                        {
                            Database db = server.Databases[Helper.Properties.Settings.Default.DefaultDBName];
                            User u = db.Users[credential.UserId];
                            foreach (DatabaseRole dbr in db.Roles)
                            {
                                if (!dbr.IsFixedRole)
                                    if (u.IsMember(dbr.Name))
                                        dbr.DropMember(u.Name);
                            }
                            u.AddToRole(userRole.ToString());
                            u.Alter();

                            succ = UpdateUserInfo(credential, userRole).Result;
                        }
                    }
                    return succ;
                }
                catch { return false; }
            });
        }

        public static Task<bool> RemoveUserAsync(string userID)
        {
            return Task.Factory.StartNew<bool>(() =>
            {
                SqlConnection conn = DataAccessHelper.CreateConnection();
                try
                {
                    bool succ;
                    using (conn)
                    {
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        var login = server.Logins[userID];
                        login.Drop();

                        Database db = server.Databases[Helper.Properties.Settings.Default.DefaultDBName];
                        User u = db.Users[userID];

                        u.Drop();

                        succ = RemoveUserInfo(userID).Result;
                    }
                    return succ;
                }
                catch { return false; }
            });
        }

        public static ObservableCollection<UserRole> GetUserRolesForDisplay()
        {
            return new ObservableCollection<UserRole>() { UserRole.None, UserRole.User, UserRole.Teacher, UserRole.Accounts, UserRole.Deputy, UserRole.Principal };
        }
    }
}
