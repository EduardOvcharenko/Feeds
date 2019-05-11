using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds.Controllers.Models
{
    public class CreateCollectionRequest
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "collectionName")]
        public string  CollectionName { get; set; }
    }
}
