using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils.Spider
{
    public class PerformGrab
    {
        public PerformGrab(AGrab grab) 
        {
            
        }
        public AGrab CreateGrab(string name)
        {
            var type = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && typeof(AGrab).IsAssignableFrom(t));
            if (type != null)
            {
                return Activator.CreateInstance(type) as AGrab;
            }
            return null;
        }
    }
}
