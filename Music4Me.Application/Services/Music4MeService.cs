using Microsoft.Extensions.Caching.Memory;
using Music4Me.Client.Resources;
using Music4Me.Common.Extensions;
using Music4Me.Common.Infrastructure.Spotify;
using Music4Me.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music4Me.Application.Services
{
    public class Music4MeService : IMusic4MeService
    {
        private IMusicService musicService;
        private readonly IMemoryCache cache;

        public Music4MeService(
            IMusicService musicService,
            IMemoryCache cache)
        {
            this.musicService = musicService;
            this.cache = cache;
        }

        public async Task<IList<Category>> GetCategories(string country)
        {
            var cacheKey = $"Music4Me.Categories.Country-{country}";
            var categories = this.cache.Get<IList<Category>>(cacheKey);

            if (categories == null) {
                categories = await this.musicService.GetCategories<Category>(country);

                // Save to cache
                this.cache.Set(cacheKey, categories, new DateTimeOffset(DateTime.Now.AddMinutes(30)));
            }
            return categories;
        }

        public async Task<IList<Artist>> GetArtists(string category, string country, int? maxCount = null)
        {
            var cacheKey = $"Music4Me.Artists.{category}.Country-{country}.MaxCount-{maxCount}";
            var artists = this.cache.Get<IList<Artist>>(cacheKey);

            if (artists == null) {
                artists = await this.musicService.GetArtists<Artist>(category, country);

                if (maxCount.HasValue) {
                    artists = GetTopArtists(artists, maxCount.Value);
                }

                // Save to cache
                this.cache.Set(cacheKey, artists, new DateTimeOffset(DateTime.Now.AddMinutes(30)));
            }
            return artists;
        }

        public async Task<IList<Artist>> GetRecommendedArtists(
            Selections selections,
            int? maxCount = null)
        {
            var artists = await this.musicService.GetRelatedArtists<Artist>(selections.LikedArtitsts);

            // Remove dublicates
            artists = artists.GroupBy(a => a.Id)
                             .Select(a => a.First())
                             .ToList();

            // Remove disliked artists
            if (selections.DislikedArtitsts != null) {
                foreach (var dislikedArtistId in selections.DislikedArtitsts) {
                    var dislikedArtist = artists.FirstOrDefault(a => a.Id == dislikedArtistId);
                    if (dislikedArtist != null) {
                        artists.Remove(dislikedArtist);
                    }
                }
            }
            // Return top artists
            if (maxCount.HasValue) {
                artists = GetTopArtists(artists, maxCount.Value);
            }

            return artists;
        }

        /// <summary>
        /// Get top most popular artists
        /// </summary>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        private IList<Artist> GetTopArtists(IList<Artist> artists, int maxCount)
        {
            return artists.OrderByDescending(a => a.Popularity)
                          .Take(maxCount)
                          .ToList();
        }
    }
}
