using BeginSEO.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BeginSEO.Data.DataEnum;

namespace BeginSEO.Data
{
    [database]
    public class KeyWord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        /// <summary>
        /// 关键词类型
        /// </summary>
        public bool Type { get; set; }
        /// <summary>
        /// 替换关键词的优先级，数值高的在最后替换反之亦然
        /// </summary>
        public int level { get; set; } = 0;
    }
}
