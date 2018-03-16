
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UmanyiSMS.Lib.Security;

namespace UmanyiSMS.Lib.Controllers
{
    public static class ActivationHelper
    {

        static void CheckLicense()
        {
            if (RegistryHelper.GetKeyValue("adata") == null)
                return;
            if (RegistryHelper.GetKeyValue("ah") == null)
                return;
            string s = RegistryHelper.GetKeyValue("adata").ToString();
            if (s.Length < 29)
                return;
            if (Security.DataProtection.GetSha1Hash(s) != RegistryHelper.GetKeyValue("ah").ToString())
                return;
            if (s[0] == 'X')
                return;
            if (s.Length > 29)
            {
                if (UmanyiSMS.Lib.Properties.Settings.Default.DBLogFilePath != DateTime.Now.Date.ToString())
                {
                    int i = int.Parse(s.Substring(29)) + 1;
                    s = s.Substring(0, 29) + i;
                    RegistryHelper.SetKeyValue("adata", s);
                    RegistryHelper.SetKeyValue("ah", Security.DataProtection.GetSha1Hash(s));
                    UmanyiSMS.Lib.Properties.Settings.Default.DBLogFilePath = DateTime.Now.Date.ToString();
                }
            }
            else
            {
                s = s + "0";
                RegistryHelper.SetKeyValue("adata", s);
                RegistryHelper.SetKeyValue("ah", Security.DataProtection.GetSha1Hash(s));
            }
        }

		public  static Task<bool> IsActivated()
		{    
			return Task<bool>.Factory.StartNew(() => {
				CheckLicense();
				var t = true;
				string s = RegistryHelper.GetKeyValue("adata").ToString();
				
				if (s[0] == 'X') {
					t = (DataAccessHelper.Helper as SqlServerHelper).SetOffline().Result;
					var y=(DataAccessHelper.Helper as SqlServerHelper).SetOnline().Result;
					return true;}
				else if (RegistryHelper.GetKeyValue("ah") == null) {
					t = (DataAccessHelper.Helper as SqlServerHelper).SetOffline().Result; 
					return false; 
				}				
				else if (s.Length < 29) {
					t = (DataAccessHelper.Helper as SqlServerHelper).SetOffline().Result; 
					return false;
				}
				else if (Security.DataProtection.GetSha1Hash(s) != RegistryHelper.GetKeyValue("ah").ToString()) {
					t = (DataAccessHelper.Helper as SqlServerHelper).SetOffline().Result; 
					return false;
				}
				else if (RegistryHelper.GetKeyValue("adata") == null) {
					t = (DataAccessHelper.Helper as SqlServerHelper).SetOffline().Result; 
					return false; 
				}				
				else if ((s.Length > 29) && (int.Parse(s.Substring(29)) > 30)) {
					t = (DataAccessHelper.Helper as SqlServerHelper).SetOffline().Result; 
					return false;
				}
				
				return true;     
			});
		}

        private async static Task<bool> DeActivate()
        {
            string s1 = DataProtection.GetSha1Hash(SystemInfo.MotherBoardSerial);
            JObject res = await WebHelper.SendPostRequest("http://umanyi.co.ke/activation/", new List<KeyValuePair<string, string>>()
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
