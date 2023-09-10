using BeginSEO.Data;
using BeginSEO.Utils._5118.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils._5118
{
    internal class ReplaceKeyWordHandle : I5118Handle
    {
        public List<KeyWord> keyWords { get; set; }
        public string InnerText { get; set; }
        public ReplaceKeyWordHandle(List<KeyWord> _keyWords, string _innerText)
        {
            keyWords = _keyWords;
            InnerText = _innerText;
        }
        public async Task<_5118Result> HandleText()
        {
            if (string.IsNullOrEmpty(InnerText))
            {
                throw new Exception("文本内容不能为空");
            }

            var FilterKeywords = keyWords
                .Where(I => I.Type != true)
                .OrderByDescending(I => I.Key.Length)
                .ThenBy(I => I.level)
                .ToList();

            var random = new Random(); // 创建一个随机数生成器，以便避免在循环中多次创建

            foreach (var item in FilterKeywords)
            {
                string[] splitText = item.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitText.Length == 0) continue; // 跳过没有值的关键词

                string newString = splitText[random.Next(splitText.Length)];
                InnerText = InnerText.Replace(item.Key, newString);
            }
            return new _5118Result
            {
                code = _5118Code.Success,
                msg = InnerText,
                like = 100
            };
        }
    }
}
