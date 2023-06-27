using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BeginSEO.Data;
using BeginSEO.Data.DataEnum;
using BeginSEO.SQL;
using BeginSEO.Utils._5118;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BeginSEO.Utils
{
    public static class _5188Tools
    {
        const string DetectionUrl = "https://apis.5118.com/wyc/original";
        const string OriginalUrl = "http://apis.5118.com/wyc/rewrite";
        const string ReplaceKeyWordUrl = "http://apis.5118.com/wyc/akey";
        const string NewOriginalUrl = "http://apis.5118.com/wyc/seniorrewrite";
        /// <summary>
        /// 检测文本原创度
        /// </summary>
        /// <returns></returns>
        public static _5118Request Detection(string key)
        {
            return new _5118Request(key, DetectionUrl);
        }
        public static async Task<Detection> DetectionRequest(this _5118Request request, string value)
        {
            return await request.Request<Detection>(new Dictionary<string, string>()
            {
                {"txt", Uri.UnescapeDataString(WebUtility.UrlEncode(value))}
            });
        }
        /// <summary>
        /// 智能原创
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="strict"></param>
        /// <returns></returns>
        public static _5118Request Original(string key)
        {
            return new _5118Request(key, OriginalUrl);
        }
        public static async Task<Original> OriginalRequest(this _5118Request request, string value, string strict)
        {
            return await request.Request<Original>(new Dictionary<string, string>()
            {
                {"txt", Uri.UnescapeDataString(WebUtility.UrlEncode(value))},
                {"sim", "1" },
                {"strict",strict}
            });
        }
        /// <summary>
        /// 替换关键词
        /// </summary>
        /// <param name="value"></param>
        /// <param name="filter">设置锁词可在一键智能原创时锁住这些词不被替换(用‘|’隔开)</param>
        /// <returns></returns>
        public static _5118Request Akey(string key)
        {
            return new _5118Request(key, ReplaceKeyWordUrl);
        }
        public static async Task<Akey> AkeyRequest(this _5118Request request, string value, string filter)
        {
            return await request.Request<Akey>(new Dictionary<string, string>()
            {
                {"txt", Uri.UnescapeDataString(WebUtility.UrlEncode(value))},
                {"sim", "1" },
                {"corewordfilter","1" },
                {"filter",filter}
            });
        }
        /// <summary>
        /// 智能原创升级版
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static _5118Request NewOriginal(string key)
        {
            return new _5118Request(key, NewOriginalUrl);
        }
        public static async Task<Original> NewOriginalRequest(this _5118Request request, string value)
        {
            // 改之前
            //return await request.Request<Original>(new Dictionary<string, string>()
            //{
            //    {"txt", WebUtility.UrlEncode(value)},
            //    {"sim", "1" },
            //    {"keephtml", "true"}
            //});
            // 改之后，使用Uri.UnescapeDataString处理UrlEncode后的字符串
            return await request.Request<Original>(new Dictionary<string, string>()
            {
                {"txt", Uri.UnescapeDataString(WebUtility.UrlEncode(value))},
                {"sim", "1" },
                {"keephtml", "true"}
            });
        }
    }
}
