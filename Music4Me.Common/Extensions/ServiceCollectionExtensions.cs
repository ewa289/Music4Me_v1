using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Music4Me.Common.Application;
using System;
using System.Collections.Generic;
using System.Text;

namespace Music4Me.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static T AddSettings<T>(
            this IServiceCollection services,
            IConfiguration configuration) where T : class, new()
        {
            var settingsInstance = new T();
            configuration.GetSection(settingsInstance.GetType().Name)
                         .Bind(settingsInstance, binderOptions => binderOptions.BindNonPublicProperties = true);

            services.AddSingleton(settingsInstance);
            return settingsInstance;
        }
    }
}
