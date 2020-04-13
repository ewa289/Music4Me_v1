using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Music4Me.Common.Infrastructure.Spotify
{
    public class SpotifyToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// The time period (in seconds) for which the access token is valid
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpirationPeriod { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
