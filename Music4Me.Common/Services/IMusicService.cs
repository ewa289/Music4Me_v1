using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Music4Me.Common.Services
{
    public interface IMusicService
    {
        Task<IList<T>> GetCategories<T>(string country);
        Task<IList<T>> GetArtists<T>(string category, string country);
        Task<IList<T>> GetRelatedArtists<T>(IList<string> artistIds);
    }
}
