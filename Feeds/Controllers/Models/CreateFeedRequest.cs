using Feeds.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds.Controllers.Models
{
    public class CreateFeedRequest
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public string Tags { get; set; }
    }
}
