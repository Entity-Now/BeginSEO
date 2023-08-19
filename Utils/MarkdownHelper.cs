using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMark;

namespace BeginSEO.Utils
{
    public static class MarkdownHelper
    {
        /// <summary>
        /// 将Markdown转换为Html
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MarkToHtml(string source)
        {
            return CommonMark.CommonMarkConverter.Convert(source);
        }
    }
}
