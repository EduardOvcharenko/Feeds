using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds.Models
{
    public class Feed
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }

        public List<Post> Posts { get; set; }

        public List<CollectionFeeds> CollectionFeeds { get; set; }
        public Feed() => CollectionFeeds = new List<CollectionFeeds>();
    }
}
