using Microsoft.Win32;
using System;
using System.Linq;


namespace Helper
{
    public class RegistryHelper
    {
        public static bool CheckSQLServer()
        {
            try
            {
                bool tempCls = IsSQLServerInstanceInstalled();
                if (GetSQLVersionInstalled() < new Version("10.00.0000.0"))
                    tempCls = false;
                return tempCls;
            }
            catch { return false; }
        }

        private static bool IsSQLServerInstanceInstalled()
        {
            try
            {
                RegistryKey rekey = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Microsoft").
                    OpenSubKey("Microsoft SQL Server").
                    OpenSubKey("Umanyi");
                return true;
            }
            catch { return false; }
            //"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server\Instance Names\SQL
            //HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server\UMANYI"
            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\UMANYI\MSSQLServer\CurrentVersion
        }

        private static Version GetSQLVersionInstalled()
        {
            Version tempCls = new Version();
            try
            {
                RegistryKey rekey = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Microsoft").
                    OpenSubKey("Microsoft SQL Server").
                    OpenSubKey("Umanyi").OpenSubKey("MSSQLServer").OpenSubKey("CurrentVersion");
                string res = rekey.GetValue("CurrentVersion") as string;
                Version.TryParse(res, out tempCls);
            }
            catch { }
            return tempCls;
        }


        public static void CheckRegistryKeys()
        {
            if (!KeysExist())
            {
                CreateKeys();
            }
        }

        public static bool CreateKeys()
        {
            try
            {
                RegistryKey MainKey =
                    Registry.LocalMachine.OpenSubKey("SOFTWARE").CreateSubKey("Umanyi").
                    CreateSubKey(Properties.Settings.Default.ApplicationName).CreateSubKey("Updates");
                

                return true;
            }
            catch
            { return false; }
        }
        
        private static bool KeysExist()
        {
            try
            {
                RegistryKey existingMainKey =
                    Registry.LocalMachine.OpenSubKey("SOFTWARE").
                    OpenSubKey("Umanyi").OpenSubKey(Properties.Settings.Default.ApplicationName).OpenSubKey("Updates");                
                
                return true;
            }
            catch { return false; }
        }
    }
}




    