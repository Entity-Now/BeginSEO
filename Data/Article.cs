using BeginSEO.Data.DataEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Data
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        /// <summary>
        /// 文章原地址，此链接的目的是相同的文章不抓取第二次。
        /// </summary>
        public string Url { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        /// <summary>
        /// 智能原创后的文章
        /// </summary
        public string Rewrite { get; set; }
        /// <summary>
        /// 相似度
        /// </summary>
        public double Contrast { get; set; }
        /// <summary>
        /// 此篇文章是否已经使用
        /// </summary>
        public bool IsUse { get; set; }
        public bool IsUseRewrite { get; set; }
        public bool IsUseReplaceKeyword { get; set; }
        /// <summary>
        /// 是否人工检查过
        /// </summary>
        public bool IsInspect { get; set; }
        /// <summary>
        /// 文章类型
        /// </summary>
        public ArticleEnum Type { get; set; }
        public DateTime GrabTime { get; set; }
    }
}
