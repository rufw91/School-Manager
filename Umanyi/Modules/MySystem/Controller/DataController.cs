using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using UmanyiSMS.Lib.Controllers;

namespace UmanyiSMS.Modules.MySystem.Controller
{
    public class DataController
    {
        public static Task<int> GetNewID(string fullTableName)
        {
            return Task.Factory.StartNew<int>(() =>
            {
                string selectStr = "SELECT dbo.GetNewID(@tbName)";
                var paramColl = new List<SqlParameter>() { new SqlParameter("@tbName", fullTableName) };
                string finalStr = DataAccessHelper.Helper.ExecuteScalar(selectStr,paramColl);
                int res;
                int.TryParse(finalStr, out res);
                return res;
            });
        }

    }
}
