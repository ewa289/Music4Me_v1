using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Music4Me.Common.Infrastructure.Spotify.Resources
{
    public class PlaylistItem
    {
        [JsonProperty("track")]
        public PlaylistTrack Track { get; set; }
    }
}
