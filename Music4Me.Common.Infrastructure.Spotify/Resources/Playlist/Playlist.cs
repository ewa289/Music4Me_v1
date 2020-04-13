using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Music4Me.Common.Infrastructure.Spotify.Resources
{
    public class Playlist
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tracks")]
        public PlaylistTracksUrl TracksUrl { get; set; }
    }
}
