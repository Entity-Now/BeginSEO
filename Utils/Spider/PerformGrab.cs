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
        public PerformGrab(IGrabRequest grab) 
        {
            
        }
        public IGrabRequest CreateGrab(string name)
        {
            var type = Assembly.GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && typeof(IGrabRequest).IsAssignableFrom(t));
            if (type != null)
            {
                return Activator.CreateInstance(type) as IGrabRequest;
            }
            return null;
        }
    }
}
