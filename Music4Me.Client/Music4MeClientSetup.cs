using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Music4Me.Client.Infrastructure;
using Music4Me.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Music4Me.Client
{
    public static class Music4MeClientSetup
    {
        public static void AddMusic4MeClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IMusic4MeClient, Music4MeClient>();

            services.AddSettings<Music4MeClientSettings>(configuration);
        }
    }
}
