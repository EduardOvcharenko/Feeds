using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds.Controllers.Models
{
    public class AddFeedToCollectionRequest
    {
        [JsonProperty(PropertyName = "collectionId")]
        public string CollectionId { get; set; }

        [JsonProperty(PropertyName = "feedId")]
        public string FeedId { get; set; }
    }
}
