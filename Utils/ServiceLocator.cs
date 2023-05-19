using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeginSEO.Utils
{
    public static class ServiceLocator
    {
        public static IServiceProvider serviceProvider;

        public static void Initialize(IServiceProvider provider)
        {
            serviceProvider = provider;
        }

        public static T GetService<T>()
        {
            return serviceProvider.GetRequiredService<T>();
        }
    }

}
