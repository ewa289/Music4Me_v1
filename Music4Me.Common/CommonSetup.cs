using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Music4Me.Common
{
    public static class CommonSetup
    {
        public static void AddCommonServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddMemoryCache();
        }
    }
}
