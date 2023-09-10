using BeginSEO.Data.DataEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils._5118
{
    public class _5118Config
    {
        public string url;
        public string key;
        public SettingsEnum type;
        public Dictionary<string, string> parameters;
        public string GetUrl()
        {
            return url;
        }
        public string GetKey()
        {
              return key;
        }
        public Dictionary<string, string> GetParameters()
        {
            return parameters;
        }
        public SettingsEnum GetType()
        {
            return type;
        }
        public void SetUrl(string value)
        {
            url = value;
        }
        public void SetKey(string value)
        {
            key = value;
        }
        public void SetParameters(Dictionary<string, string> value)
        {
            parameters = value;
        }
        public void SetType(SettingsEnum value)
        {
            type = value;
        }
    }
}
