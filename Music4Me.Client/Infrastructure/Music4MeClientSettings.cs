using Music4Me.Common.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Music4Me.Client.Infrastructure
{
    public class Music4MeClientSettings : ApiClientSettings
    {
        public int MaxArtistsCount { get; set; }
        public int MaxRecommendations { get; set; }
        public string Country { get; set; } = "se";
    }
}
