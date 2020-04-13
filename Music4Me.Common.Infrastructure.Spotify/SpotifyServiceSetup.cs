using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Music4Me.Common.Extensions;
using Music4Me.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Music4Me.Common.Infrastructure.Spotify
{
    public static class SpotifyServiceSetup
    {
        public static void AddSpotifyService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSettings<SpotifyServiceSettings>(configuration);

            services.AddTransient<IMusicService, SpotifyService>();
        }
    }
}
