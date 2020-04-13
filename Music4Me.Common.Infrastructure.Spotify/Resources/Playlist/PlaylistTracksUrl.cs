using Newtonsoft.Json;

namespace Music4Me.Common.Infrastructure.Spotify.Resources
{
    public class PlaylistTracksUrl
    {
        [JsonProperty("href")]
        public string Href { get; set; }
    }
}