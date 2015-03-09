using Helper.Security;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Helpers
{
    public static class ActivationHelper
    {
        public static bool CheckLicense()
        {
            return true;
        }
        public async static Task<bool> Activate(string license)
        {
                string s1=DataProtection.GetSha1Hash(SystemInfo.MotherBoardSerial);
                string s2 = DataProtection.GetSha1Hash(license);
                JObject res=await WebHelper.SendPostRequest("http://monsoondigital.co.ke/activation/",new List<KeyValuePair<string,string>>()
                {
                    new KeyValuePair<string,string>("tag","activate"),
                    new KeyValuePair<string,string>("s1",s1),
                    new KeyValuePair<string,string>("s2",s2)
                });
                if (res == null)
                    return false;
                if (res["success"].ToString() == "1")
                    return true;
                else return false;
        }

        public static Task<bool> IsActivated()
        {
            return Task.Run<bool>(() => { return true; });
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
