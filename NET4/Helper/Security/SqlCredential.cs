using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace Helper.Security
{
    public sealed class SqlCredential
    {
        string userId;
        SecureString pwd;

        public SqlCredential(string userID, SecureString password)
        {
            userId = userID;
            pwd = password;
        }
        public string UserId { get{return userId; }}

        public SecureString Password { get{return pwd; } }
    }
}
