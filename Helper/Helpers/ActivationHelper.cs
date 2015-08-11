using Helper.Security;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Helper.Helpers
{
    public static class ActivationHelper
    {

        static void CheckLicense()
        {
            if (RegistryHelper.GetKeyValue(null, "adata") == null)
                return;
            if (RegistryHelper.GetKeyValue(null, "ah") == null)
                return;
            string s = RegistryHelper.GetKeyValue(null, "adata").ToString();
            if (s.Length < 29)
                return;
            if (Security.DataProtection.GetSha1Hash(s) != RegistryHelper.GetKeyValue(null, "ah").ToString())
                return;
            if (s[0] == 'X')
                return;
            if (s.Length > 29)
            {
                if (Helper.Properties.Settings.Default.DBLogFilePath != DateTime.Now.Date.ToString())
                {
                    int i = int.Parse(s.Substring(29)) + 1;
                    s = s.Substring(0, 29) + i;
                    RegistryHelper.SetKeyValue(null, "adata", s);
                    RegistryHelper.SetKeyValue(null, "ah", Security.DataProtection.GetSha1Hash(s));
                    Helper.Properties.Settings.Default.DBLogFilePath = DateTime.Now.Date.ToString();
                }
            }
            else
            {
                s = s + "0";
                RegistryHelper.SetKeyValue(null, "adata", s);
                RegistryHelper.SetKeyValue(null, "ah", Security.DataProtection.GetSha1Hash(s));
            }
        }

        public async static Task<bool> LicenseExists()
        {
            if (RegistryHelper.GetKeyValue(null, "adata") == null)
            { await DataAccessHelper.SetOffline(); return false; }
            if (RegistryHelper.GetKeyValue(null, "ah") == null)
            { await DataAccessHelper.SetOffline(); return false; }
            return true;
        }

        public async static Task<bool> IsActivated()
        {    
                CheckLicense();
                if (RegistryHelper.GetKeyValue(null, "adata") == null)
                { await DataAccessHelper.SetOffline(); return false; }
                if (RegistryHelper.GetKeyValue(null, "ah") == null)
                {  await DataAccessHelper.SetOffline(); return false;}
                string s = RegistryHelper.GetKeyValue(null, "adata").ToString();
                if (s.Length < 29)
                { await DataAccessHelper.SetOffline();   return false;}
                if (Security.DataProtection.GetSha1Hash(s) != RegistryHelper.GetKeyValue(null, "ah").ToString())
                { await DataAccessHelper.SetOffline();  return false;}
                if (s[0] == 'X')
                { await DataAccessHelper.SetOffline();  return false;}
                if ((s.Length > 29) && (int.Parse(s.Substring(29)) > 30))
                { await DataAccessHelper.SetOffline();  return false;}
                return true;             
        }

        private async static Task<bool> DeActivate()
        {
            string s1 = DataProtection.GetSha1Hash(SystemInfo.MotherBoardSerial);
            JObject res = await WebHelper.SendPostRequest("http://monsoondigital.co.ke/activation/", new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string,string>("tag","deactivate"),
                    new KeyValuePair<string,string>("s1",s1)
                });
            if (res == null)
                return false;
            if (res["success"].ToString() == "1")
                return true;
            else return false;
        }
    }
}
