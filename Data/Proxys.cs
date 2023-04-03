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
    [database("Proxys")]
    public class Proxys
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public ProxyStatus Status { get; set; }
        public int Speed { get; set; }
    }
}
