using Microsoft.Win32;
using System;
using System.Linq;


namespace UmanyiSMS.Lib.Controllers
{
	
    public static class RegistryHelper
    {
        const string guid = "{DBA3C969-6B84-495D-9D5B-03DD8D4FFC5C}";
        static RegistryHelper()
        {
            try
            {
                    Registry.CurrentUser.OpenSubKey("SOFTWARE",true).CreateSubKey("Umanyi").
                    CreateSubKey("Umanyi Digital Technologies").CreateSubKey(guid);
                
            }
            catch{ }
        
        }
        internal static object GetKeyValue(string name)
        {
            object val = null;
            try
            {
                val = Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Umanyi").OpenSubKey("Umanyi Digital Technologies")
                    .OpenSubKey(guid).GetValue(name);
            }
            catch { }
            return val;
        }

        internal static bool SetKeyValue(string name,object value)
        {
           
            try
            {
                    Registry.CurrentUser.OpenSubKey("SOFTWARE",true).OpenSubKey("Umanyi",true).OpenSubKey("Umanyi Digital Technologies",true)
                    .OpenSubKey(guid,true).SetValue(name,value) ;
                return true;
            }
            catch { }
            return false;
        }
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
                tempCls = Version.Parse(res);
            }
            catch { }
            return tempCls;
        }

        public static bool IsFirstRun()
        {
            var obj = GetKeyValue("FirstRun");
            if (obj != null)
            {
                if (obj.ToString() == "0")
                    return false;
                else if (obj.ToString() == "1")
                    return true;
                else throw new Exception("Invalid Value");
            }
            else
                return false;
        }

        public static bool SetFirstRunComplete()
        {
            return SetKeyValue("FirstRun", 0);
        }
    }
}




    