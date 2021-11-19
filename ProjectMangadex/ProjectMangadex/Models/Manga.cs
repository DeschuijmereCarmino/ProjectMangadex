using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ProjectMangadex.Models
{
    //definieren van een manga object
    public class Manga
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        public string Cover { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [JsonProperty(PropertyName = "relationships")]
        public List<Relationship> Relationships { get; set; }

        [JsonExtensionData]
        private Dictionary<string, JToken> _extraJsonData = new Dictionary<string, JToken>();

        [OnDeserialized]
        private void ProcessExtraJSonData(StreamingContext context)
        {
            JToken jsonTitle = (JToken)_extraJsonData["attributes"]["title"];
            JToken jsonDescription = (JToken)_extraJsonData["attributes"]["description"];

            Title = (string)jsonTitle.SelectToken("en");
            Description = (string)jsonDescription.SelectToken("en");
        }
    }

    public class Relationship
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
