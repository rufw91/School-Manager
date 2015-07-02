using Microsoft.Win32;

namespace ActivateMe
{
    internal static class RegistryHelper
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
        internal static object GetKeyValue(string key, string name)
        {
            object val = null;
            try
            {
                val = string.IsNullOrEmpty(key) ? Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Umanyi").OpenSubKey("Umanyi Digital Technologies")
                    .OpenSubKey(guid).GetValue(name) : Registry.CurrentUser.OpenSubKey("SOFTWARE").OpenSubKey("Umanyi").OpenSubKey("Umanyi Digital Technologies")
                    .OpenSubKey(guid).OpenSubKey(key).GetValue(name);
            }
            catch { }
            return val;
        }

        internal static bool SetKeyValue(string key, string name,object value)
        {
           
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                    Registry.CurrentUser.OpenSubKey("SOFTWARE",true).OpenSubKey("Umanyi",true).OpenSubKey("Umanyi Digital Technologies",true)
                    .OpenSubKey(guid,true).SetValue(name,value) ;
                else
                    Registry.CurrentUser.OpenSubKey("SOFTWARE", true).OpenSubKey("Umanyi", true).OpenSubKey("Umanyi Digital Technologies", true)
                    .OpenSubKey(guid,true).OpenSubKey(key,true).SetValue(name,value);
                return true;
            }
            catch { }
            return false;
        }
    }
}
