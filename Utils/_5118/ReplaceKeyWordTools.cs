using BeginSEO.Data;
using BeginSEO.SQL;
using BeginSEO.Utils._5118;
using BeginSEO.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils
{
    public class ReplaceKeyWordTools
    {
        public List<KeyWord> keyWords { get; set; }
        public _5118Request ROriginal { get; set; }
        public _5118Request RAkey { get; set; }
        public _5118Request RNewOrignal { get; set; }
        public ReplaceKeyWordTools(List<KeyWord> _keyword)
        {
            keyWords = _keyword;
        }
        public ReplaceKeyWordTools(List<KeyWord> _keyword, _5118Request _ROriginal, _5118Request _RAkey, _5118Request rNewOrignal) : this(_keyword)
        {
            ROriginal = _ROriginal;
            RAkey = _RAkey;
            RNewOrignal = rNewOrignal;
        }
        public string replaceKeyWord(string Source, bool IsLevel = false)
        {
            if (string.IsNullOrEmpty(Source))
            {
                return Source;
            }

            var FilterKeywords = keyWords
                .Where(I => I.Type != true)
                .Where(I => !IsLevel || I.level == -1) // 使用逻辑非和简化条件判断
                .OrderByDescending(I => I.Key.Length)
                .ThenBy(I => I.level)
                .ToList();

            var random = new Random(); // 创建一个随机数生成器，以便避免在循环中多次创建

            foreach (var item in FilterKeywords)
            {
                string[] splitText = item.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitText.Length == 0) continue; // 跳过没有值的关键词

                string newString = splitText[random.Next(splitText.Length)];
                Source = Source.Replace(item.Key, newString);
            }

            return Source;
        }
        public async Task<OriginalResult> Original(string source, string strict, bool IsOriginal, bool IsReplace, bool IsNewOrinal)
        {
            try
            {
                var result = new OriginalResult
                {
                    AkeyStatus = false,
                    OriginalStatus = false,
                    NewOriginalStatus = false,
                    contrastValue = -1,
                };
                result.NewValue = replaceKeyWord(source);
                // 5118智能原创
                if (IsNewOrinal)
                {
                    var originalResult = await RNewOrignal.NewOriginalRequest(result.NewValue);
                    if (originalResult != null && originalResult.errcode == "0")
                    {
                        result.NewValue = originalResult.data;
                        result.contrastValue = float.Parse(originalResult.like) * 100;
                        result.NewOriginalError = "智能原创成功";
                        result.NewOriginalStatus = true;
                    }
                    else
                    {
                        result.NewOriginalError = originalResult.errmsg;
                    }
                }
                if (IsOriginal)
                {
                    var originalResult = await ROriginal.OriginalRequest(result.NewValue, strict);
                    if (originalResult != null && originalResult.errcode == "0")
                    {
                        result.NewValue = originalResult.data;
                        result.contrastValue = float.Parse(originalResult.like) * 100;
                        result.OriginalError = "智能原创成功";
                        result.OriginalStatus = true;
                    }
                    else
                    {
                        result.OriginalError = originalResult.errmsg;
                    }
                }
                if (IsReplace)
                {
                    var filterKeyWord = keyWords
                        .Where(kw => kw.Type)
                        .Select(kw => kw.Value.Replace(",", "|"))
                        .ToList()  // 将结果加载到内存中
                        .Aggregate("", (a, b) => string.Format("{0}|{1}", a, b));

                    var reKeyword = await RAkey.AkeyRequest(result.NewValue ?? source, filterKeyWord);
                    if (reKeyword.errcode == "0")
                    {
                        result.contrastValue = float.Parse(reKeyword.like) * 100;
                        result.NewValue = reKeyword.data;
                        result.AkeyError = "替换关键词成功";
                        result.AkeyStatus = true;
                    }
                    else
                    {
                        result.AkeyError = reKeyword.errmsg;
                    }

                }
                // 重新替换关键词
                result.NewValue = replaceKeyWord(result.NewValue);
                return result;
            }
            catch (Exception e)
            {
                throw new LoggingException($"Original {e.Message}");
            }

        }
    }
}
