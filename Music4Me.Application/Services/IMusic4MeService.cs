using Music4Me.Client.Resources;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Music4Me.Application.Services
{
    public interface IMusic4MeService
    {
        Task<IList<Category>> GetCategories(string country);
        Task<IList<Artist>> GetArtists(string category, string country, int? maxCount = null);
        Task<IList<Artist>> GetRecommendedArtists(Selections artists, int? maxCount = null);
    }
}
