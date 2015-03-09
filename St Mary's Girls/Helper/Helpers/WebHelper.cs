using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Helper.Helpers
{
    public static class WebHelper
    {
        public static async Task<JObject> SendPostRequestEx(string address, List<KeyValuePair<string, string>> data)
        {
            try
            {
                var client = new HttpClient();
                JObject j = new JObject();
                foreach (var k in data)
                {
                    j[k.Key] = k.Value;
                }
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(new Uri(address), new StringContent(j.ToString(Formatting.Indented), Encoding.UTF8, "application/json"));
                HttpResponseMessage mes = response.EnsureSuccessStatusCode();
                JObject jo = JObject.Parse(await mes.Content.ReadAsStringAsync());

                if (!mes.IsSuccessStatusCode)
                    return null;
                else return jo;
            }
            catch { }
            return null;
        }

        public static async Task<JObject> SendPostRequest(string address, List<KeyValuePair<string, string>> list)
        {
            return await Task.Run<JObject>(() =>
            {
                try
                {
                    JObject j = new JObject();
                    foreach (var k in list)
                    {
                        j[k.Key] = k.Value;
                    }
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
                    byte[] data = Encoding.UTF8.GetBytes(j.ToString());
                    request.Method = "POST";
                    request.Accept = "application/json"; //you can set application/xml
                    request.ContentType = "application/json";// you can set application/xml
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(data, 0, data.Length);
                    dataStream.Close();
                    HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
                    StreamReader result = new StreamReader(resp.GetResponseStream());
                    if (result != null)
                    {
                        var x = result.ReadToEnd().ToString();
                        MessageBox.Show(x);
                        if (!string.IsNullOrEmpty(x))
                        {
                            JObject jo = JObject.Parse(x);
                            return jo;
                        }
                    }
                }
                catch { }
                return null;
            });
        }
    }
}
