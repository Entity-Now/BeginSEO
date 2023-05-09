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
    [database]
    public class Header
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Host { get; set; }
        public string Domain { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
