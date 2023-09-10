using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Data.DataEnum
{
    public enum SettingsEnum
    {
        /// <summary>
        /// 原创度检测
        /// </summary>
        Detection,
        /// <summary>
        /// 关键词替换
        /// </summary>
        ReplaceKeyWord,
        /// <summary>
        /// 相似度查询
        /// </summary>
        Contrast,
        /// <summary>
        /// 只能原创
        /// </summary>
        Original,
        /// <summary>
        /// OpenAI的key
        /// </summary>
        OpenAi,
        /// <summary>
        /// 新版原创
        /// </summary>
        SeniorRewrite
    }
}
