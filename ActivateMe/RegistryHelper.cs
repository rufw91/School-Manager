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
    }
}
