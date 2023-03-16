using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Windows;

namespace 替换关键词.Model {
    public class KeyWord {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public static class JsonUtils
    {
        public static List<KeyWord> Ks = null;
        public static string path = "KeyWordLists.json";

        public static void OpenFile() {
            using (var reader = File.OpenText(path)) {
                var T = (JArray)JToken.ReadFrom(new JsonTextReader(reader));
                Ks = (List<KeyWord>)T.ToObject(typeof(List<KeyWord>));
            }
        }
        public static void WriteFile(List<KeyWord> data = null) {
            List<KeyWord> temp = Ks;
            if (data != null)
            {
                temp = data;
            }
            var json = JsonConvert.SerializeObject(temp);
            File.WriteAllText(path, json);
        }
        public static void update(KeyWord item)
        {
            var data = Ks.FirstOrDefault(I => I == item);
            if (data != null)
            {
                data = item;
            }
            WriteFile();
        }
        public static void update(string name, string value)
        {
            var data = Ks.FirstOrDefault(I => I.Key == name);
            if (data != null)
            {
                data.Key = name;
                data.Value = value;
                WriteFile();
            }
        }
        public static void add(KeyWord item)
        {
            var data = Ks.FirstOrDefault(I => I == item);
            if (data != null)
            {
                update(item.Key, item.Value);
                return;
            }
            Ks.Add(item);
            WriteFile();
        }
        public static void add(string name, string value)
        {
            var data = Ks.FirstOrDefault(I => I.Key == name);
            if (data != null)
            {
                update(name,value);
                return;
            }
            Ks.Add(new KeyWord()
            {
                Key = name, Value = value
            });
            WriteFile();
        }
        public static void remove(string name)
        {
            Ks.Remove(new KeyWord() { Key = name});
            WriteFile();
        }
        public static void remove(KeyWord item)
        {
            Ks.Remove(item);
            WriteFile();
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
                WriteFile(data);
            }
        }
    }
}
