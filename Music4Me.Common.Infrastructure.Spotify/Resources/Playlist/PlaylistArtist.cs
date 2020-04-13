using Newtonsoft.Json;

namespace Music4Me.Common.Infrastructure.Spotify.Resources
{
    public class PlaylistArtist
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }
}