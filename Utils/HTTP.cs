using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace 替换关键词.Utils {
    public enum RequestType {
        FormData,
        String,
        Multipart
    }
    public static class HTTP {
        public static async Task<string> Get(HttpClient client, string url) {
            var Get = await client.GetAsync(url);
            //if (!Get.IsSuccessStatusCode) return string.Empty;
            var Result = await Get.Content.ReadAsStringAsync();
            return Result;
        }
        public static async Task<string> Post(string url,Dictionary<string,string> Data,RequestType ContentType) {
            HttpClient client = new HttpClient();
            HttpContent content = null;
            if (ContentType == RequestType.FormData) {
                content = new FormUrlEncodedContent(Data);
            }
            else if(ContentType == RequestType.String) {
                string Body = "";
                foreach (var item in Data) {
                    if (string.IsNullOrEmpty(Body)) {
                        Body += $"?{item.Key}={item.Value}";
                    }
                    else {
                        Body += $"&{item.Key}={item.Value}";
                    }
                }
                content = new StringContent(Body);
            }

            var result = await client.PostAsync(url, content);
            if (!result.IsSuccessStatusCode) {
                return string.Empty;
            }
            return await result.Content.ReadAsStringAsync();
        }
    }
}
