using Music4Me.Client.Resources;
using Music4Me.Common.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Music4Me.Client.Infrastructure
{
    public class Music4MeClient : ApiClient, IMusic4MeClient
    {
        private readonly Music4MeClientSettings settings;

        public Music4MeClient(
            IHttpClientFactory httpClientFactory, 
            Music4MeClientSettings settings) : base(httpClientFactory, settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<IList<Artist>> GetArtists(string category)
        {
            return await http.Get<IList<Artist>>($"artists/{category}?maxCount={this.settings.MaxArtistsCount}&country={this.settings.Country}");
        }

        public async Task<IList<Category>> GetCategories()
        {
            return await this.http.Get<IList<Category>>($"categories?country={this.settings.Country}");
        }

        public async Task<IList<Artist>> GetRecommendations(Selections artists)
        {
            return await this.http.Post<IList<Artist>>($"recommendations?maxCount={this.settings.MaxArtistsCount}", artists);
        }
    }
}
