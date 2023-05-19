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
        public static string DetectionUrl = "https://apis.5118.com/wyc/original";
        public static string OriginalUrl = "http://apis.5118.com/wyc/rewrite";
        public static string ReplaceKeyWordUrl = "http://apis.5118.com/wyc/akey";
        /// <summary>
        /// 检测文本原创度
        /// </summary>
        /// <returns></returns>
        public static _5118Request Detection(string key, string value)
        {
            return new _5118Request(key, DetectionUrl);
        }
        public static async Task<Detection> DetectionRequest(this _5118Request request, string value)
        {
            return await request.Request<Detection>(new Dictionary<string, string>()
            {
                {"txt", WebUtility.UrlEncode(value)}
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
                {"txt", WebUtility.UrlEncode(value)},
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
                {"txt", WebUtility.UrlEncode(value)},
                {"sim", "1" },
                {"corewordfilter","1" },
                {"filter",filter}
            });
        }
    }
}
