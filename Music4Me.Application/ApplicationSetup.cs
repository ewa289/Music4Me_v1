using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Music4Me.Application.Services;
using Music4Me.Common.Infrastructure.Spotify;
using System;

namespace Music4Me.Application
{
    public static class ApplicationSetup
    {
        public static void AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddTransient<IMusic4MeService, Music4MeService>();

            services.AddSpotifyService(configuration);
        }
    }
}
