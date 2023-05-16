using BeginSEO.Data;
using BeginSEO.SQL;
using BeginSEO.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BeginSEO.Model
{
    public class ReplaceKeyWord
    {
        public async Task<string> replice(string Source = null, bool IsLevel = false)
        {
            var dataContext = DataAccess.GetDbContext();
            string Text = Source;
            if (string.IsNullOrWhiteSpace(Text))
            {
                return Text;
            }

            var list = await dataContext.Set<KeyWord>()
                .Where(I => I.Type != true)
                .Where(I => !IsLevel || I.level == -1) // 使用逻辑非和简化条件判断
                .OrderByDescending(I => I.Key.Length)
                .ThenBy(I => I.level)
                .ToListAsync();

            var random = new Random(); // 创建一个随机数生成器，以便避免在循环中多次创建

            foreach (var item in list)
            {
                string[] splitText = item.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (splitText.Length == 0) continue; // 跳过没有值的关键词

                string newString = splitText[random.Next(splitText.Length)];
                Text = Text.Replace(item.Key, newString);
            }

            return Text;
        }
        public async Task<(float, string, (string , bool), (string , bool))> Original(string source, string strict, bool IsOriginal, bool IsReplace)
        {
            (string O_Msg, bool O_Status) = ("未开启原创", false);
            (string R_Msg, bool R_Status) = ("未开启换词", false);

            string tempValue = await replice(source, true);
            float Similarty = 0;

            // 5118智能原创

            if (IsOriginal)
            {
                var originalResult = await _5188Tools.Original(tempValue, string.IsNullOrEmpty(strict) ? "0" : strict);
                if (originalResult == null || originalResult?.errcode != "0")
                {
                    (O_Msg, O_Status) = ("智能原创失败", false);
                }
                else
                {
                    Similarty = float.Parse(originalResult.like) * 100;
                    tempValue = originalResult.data;
                    (O_Msg, O_Status) = ("智能原创成功", false);
                }
            }
            if (IsReplace)
            {
                var dataContext = DataAccess.GetDbContext();
                var filterKeyWord = dataContext.Set<KeyWord>()
                .Where(kw => kw.Type)
                .Select(kw => kw.Value.Replace(",", "|"))
                .ToList()  // 将结果加载到内存中
                .Aggregate("", (a, b) => string.Format("{0}|{1}", a, b));

                var reKeyword = await _5188Tools.ReplaceKeyWord(tempValue, filterKeyWord);

                if (reKeyword == null || reKeyword?.errcode != "0")
                {
                    (R_Msg, R_Status) = ("一键换词失败", false);
                }
                else
                {
                    Similarty = float.Parse(reKeyword.like) * 100;

                    tempValue = reKeyword.data;
                    (R_Msg, R_Status) = ("一键换词成功", false);
                }

            }

            tempValue = await replice(tempValue);
            return (Similarty, tempValue, (O_Msg, O_Status), (R_Msg, R_Status));
        }
    }
}
