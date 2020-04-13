using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Music4Me.Client.Resources
{
    public class Category
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("icons")]
        public List<Image> Images { get; set; }
    }
}
