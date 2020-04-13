using Newtonsoft.Json;
using System.Collections.Generic;

namespace Music4Me.Common.Infrastructure.Spotify.Resources
{
    public class PlaylistTrack
    {
        [JsonProperty("artists")]
        public List<PlaylistArtist> Artists { get; set; }
    }
}