using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.IO;
using Newtonsoft.Json;

namespace 替换关键词.Model {
    class KeyWord {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    static class JsonUtils
    {
        public static List<KeyWord> Ks = null;
        public static string path = "KeyWordLists.json";

        public static void OpenFile() {
            using (var reader = File.OpenText(path)) {
                var T = (JArray)JToken.ReadFrom(new JsonTextReader(reader));
                Ks = (List<KeyWord>)T.ToObject(typeof(List<KeyWord>));
            }
        }
        public static void WriteFile() {
            var json = JsonConvert.SerializeObject(Ks);
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init() {
            if (File.Exists(path)) {
                OpenFile();
            }
            else {
                var data = new List<KeyWord>() 
                {
                    new KeyWord() {
                        Key = "效果",
                        Value = "成效"
                    },
                    new KeyWord() {
                        Key = "最",
                        Value = "好"
                    }
                };
                // 写出默认值
                WriteFile();
            }
        }
    }
}
