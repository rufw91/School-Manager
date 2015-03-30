using Helper.Models;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public static class DatabaseCreationHelper
    {
        static Dictionary<string,string> errors = new Dictionary<string, string>();
        public static Task<bool> CreateDefaultDatabaseAsync()
        {
            return Task.Run<bool>(() =>
            {
                string dbScript = GetDBScript();
                SqlConnection conn = DataAccessHelper.CreateConnection();
                try
                {
                    using (conn)
                    {
                        if (!Directory.Exists(Helper.Properties.Settings.Default.DBDirectoryPath))
                            Directory.CreateDirectory(Helper.Properties.Settings.Default.DBDirectoryPath);
                        var sc = new ServerConnection(conn);
                        Server server = new Server(sc);
                        foreach(Database d in server.Databases)
                            if (d.Name.ToUpperInvariant() == "STAREHE")
                            {
                                server.KillAllProcesses("Starehe");
                                server.KillDatabase("Starehe");
                            }
                        server.Databases["master"].ExecuteNonQuery(dbScript);
                    }
                    return true;
                }
                catch { }
                return false;
            });     
        }

        private static string GetDBScript()
        {
            return "";// Helper.Properties.Resources.Script;
        }
        public static Task<ObservableCollection<ColumnModel>> GetTableColumnsAsync(string nameOfTable)
        {
            return Task.Run<ObservableCollection<ColumnModel>>(() =>
                {
                    ObservableCollection<ColumnModel> temp = new ObservableCollection<ColumnModel>();
                    return temp;
                });
        }
    }
}
