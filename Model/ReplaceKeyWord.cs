using BeginSEO.Data;
using BeginSEO.SQL;
using BeginSEO.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BeginSEO.Model
{
    public class ReplaceKeyWord
    {
        public string replice(string Source = null, bool IsLevel = false)
        {
            string Text = Source;
            if (string.IsNullOrWhiteSpace(Text))
            {
                ShowToast.Show("请输入要替换的文本", ShowToast.Type.Error);
                return Text;
            }

            var list = DataAccess.Entity<KeyWord>()
                .Where(I => I.Type != true)
                .Where(I => !IsLevel || I.level == -1) // 使用逻辑非和简化条件判断
                .OrderByDescending(I => I.Key.Length)
                .ThenBy(I => I.level);

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
        public async Task<(float, string)> Original(string source, string strict, bool IsOriginal, bool IsReplace)
        {
            ShowModal.ShowLoading();

            string tempValue = replice(source, true) ?? string.Empty;
            float Similarty = 0;

            // 5118智能原创

            if (IsOriginal)
            {
                var originalResult = await _5188Tools.Original(tempValue, string.IsNullOrEmpty(strict) ? "0" : strict);
                if (originalResult?.errcode != "0")
                {
                    await ShowToast.Show("智能原创失败", ShowToast.Type.Error);
                }
                else
                {
                    Similarty = float.Parse(originalResult.like) * 100;
                    tempValue = originalResult.data;
                    await ShowToast.Show("智能原创成功", ShowToast.Type.Success);
                }
            }
            if (IsReplace)
            {
                var filterKeyWord = DataAccess.Entity<KeyWord>().Where(kw => kw.Type)
                    .Select(kw => kw.Value.Replace(",", "|"))
                    .Aggregate(string.Empty, (a, b) => $"{a}|{b}");

                var reKeyword = await _5188Tools.ReplaceKeyWord(tempValue, filterKeyWord);

                if (reKeyword?.errcode != "0")
                {
                    await ShowToast.Show("一键换词失败", ShowToast.Type.Error);
                }
                else
                {
                    Similarty = float.Parse(reKeyword.like) * 100;

                    tempValue = reKeyword.data;
                    await ShowToast.Show("一键换词成功", ShowToast.Type.Success);
                }
            }

            replice(tempValue);

            ShowModal.Closing();
            return (Similarty, tempValue);
        }
    }
}
