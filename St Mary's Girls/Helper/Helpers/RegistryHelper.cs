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
                    OpenSubKey("STAREHE");
                return true;
            }
            catch { return false; }
            //"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server\Instance Names\SQL
            //HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server\STAREHE"
            //HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Microsoft SQL Server\STAREHE\MSSQLServer\CurrentVersion
        }

        private static Version GetSQLVersionInstalled()
        {
            Version tempCls = new Version();
            try
            {
                RegistryKey rekey = Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Microsoft").
                    OpenSubKey("Microsoft SQL Server").
                    OpenSubKey("STAREHE").OpenSubKey("MSSQLServer").OpenSubKey("CurrentVersion");
                string res = rekey.GetValue("CurrentVersion") as string;
                Version.TryParse(res, out tempCls);
            }
            catch { }
            return tempCls;
        }

        public static bool CheckIfUpdateAlreadyApplied(string UpdateID)
        {
            try
            {
                bool r1;
                if (Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Hilltop SBFS").
                    OpenSubKey(Properties.Settings.Default.ApplicationName).OpenSubKey("Updates").
                    GetValueNames().Contains(UpdateID, StringComparer.InvariantCultureIgnoreCase))
                {
                    bool.TryParse(Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Hilltop SBFS").
                    OpenSubKey(Properties.Settings.Default.ApplicationName).OpenSubKey("Updates").
                    GetValue(UpdateID, false).ToString(), out r1);
                    return r1;
                }
                else
                    Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Hilltop SBFS").
                    OpenSubKey(Properties.Settings.Default.ApplicationName).OpenSubKey("Updates", true).SetValue(UpdateID, false);
            }
            catch
            {
                
            } return false;
        }

        public static void StoreTemp(string text)
        {
            try
            {
                Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Hilltop SBFS").
                    OpenSubKey(Properties.Settings.Default.ApplicationName).OpenSubKey("Updates", true).
                    SetValue(Guid.NewGuid().ToString(), text);
            }
            catch { }
        }

        public static bool SetUpdateAlreadyApplied(string UpdateID)
        {
            try
            {
                Registry.LocalMachine.OpenSubKey("SOFTWARE").OpenSubKey("Hilltop SBFS").
                    OpenSubKey(Properties.Settings.Default.ApplicationName).OpenSubKey("Updates", true).
                    SetValue(UpdateID, true);
                return true;
            }
            catch { return false; }
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
                    Registry.LocalMachine.OpenSubKey("SOFTWARE").CreateSubKey("Hilltop SBFS").
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
                    OpenSubKey("Hilltop SBFS").OpenSubKey(Properties.Settings.Default.ApplicationName).OpenSubKey("Updates");                
                
                return true;
            }
            catch { return false; }
        }
    }
}




    