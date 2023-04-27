using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils.Spider
{
    public interface IGrabRequest
    {
        string URL { get; set; }
        /// <summary>
        /// 文章截取模板
        /// </summary>
        string ReplaceTemplate { get; set; }
        /// <summary>
        /// 抓取文章
        /// </summary>
        /// <returns></returns>
        ArticleData Grab();
    }
}
