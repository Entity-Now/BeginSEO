using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Attributes
{
    public class ExcellDataAttribute: Attribute
    {
        public List<string> Headers { get; set; }
        public ExcellDataAttribute(params string[] _header)
        {
            this.Headers = _header.ToList();
        }
    }
}
