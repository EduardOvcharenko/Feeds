using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds.Controllers.Models
{
    public class RenameCollectionRequest
    {
        [JsonProperty(PropertyName = "collectionId")]
        public string CollectionId { get; set; }

        [JsonProperty(PropertyName = "newName")]
        public string NewName { get; set; }
    }
}
