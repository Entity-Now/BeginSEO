using BeginSEO.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Data
{
    /// <summary>
    /// 存储cookie信息
    /// </summary>
    [database]
    public class TempCookie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string Host { get; set; }
        public string CookieKey { get; set; }
        public string CookieValue { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime TopTime { get; set; }
    }
}
