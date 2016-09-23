using Helper.Security;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Helper
{
    public abstract class DBHelper
    {
        internal abstract bool TestCredential(SqlCredential newCredentials);
        internal abstract void SetCredential(SqlCredential newCredentials);
        public abstract dynamic CreateConnection();
        internal abstract dynamic CreateConnection(string connectionString);
        public abstract string ExecuteScalar(string commandText);
        public abstract string ExecuteScalar(string commandText, IEnumerable<DbParameter> paramColl);
        public abstract object ExecuteObjectScalar(string commandText);
        public abstract object ExecuteObjectScalar(string commandText, IEnumerable<DbParameter> paramColl);
        public abstract DataTable ExecuteNonQueryWithResultTable(string commandText, IEnumerable<DbParameter> paramColl);
        protected DataTable GetResultTable(DbDataReader reader)
        {
            DataTable dt = new DataTable();
            if (!reader.IsClosed&&reader.HasRows)
                
            {
                int colIndex = reader.FieldCount;
                for (int i = 0; i < colIndex; i++)
                {
                    dt.Columns.Add(new DataColumn("", typeof(object)));
                }
                DataRow dtr;
                while (reader.Read())
                {
                    dtr = dt.NewRow();
                    for (int i = 0; i < colIndex; i++)
                        dtr[i] = reader.GetValue(i);
                    dt.Rows.Add(dtr);
                }
            }
            return dt;
        }

        protected List<string> GetResultFirstCol(DbDataReader reader)
        {
            List<string> dt = new List<string>();
            if (!reader.IsClosed && reader.HasRows)
            {
                int colIndex = reader.FieldCount;
                while (reader.Read())
                {
                    dt.Add(reader.GetValue(0).ToString());
                }
            }
            return dt;
        }
        public abstract bool ExecuteNonQuery(string commandText, IEnumerable<DbParameter> paramColl);
        public abstract DataTable ExecuteNonQueryWithResultTable(string commandText);
        public abstract bool ExecuteNonQuery(string commandText);
        public abstract List<string> CopyFirstColumnToList(string commandText);
        public abstract List<string> CopyFirstColumnToList(string commandText, IEnumerable<DbParameter> paramColl);
    }
}
