using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Attributes
{
    class databaseAttribute: Attribute
    {
        public string Table { get; set; }
        public databaseAttribute(string _table)
        {
            Table = _table;
        }
        public databaseAttribute() { }
    }
}
