using Music4Me.Client.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Music4Me.Client
{
    public interface IMusic4MeClient
    {
        Task<IList<Artist>> GetArtists(string category);
        Task<IList<Category>> GetCategories();
        Task<IList<Artist>> GetRecommendations(Selections artist);
    }
}
