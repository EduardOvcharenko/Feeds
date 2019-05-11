using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds.Models
{
    public class UserCollection
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public List<CollectionFeeds> CollectionFeeds { get; set; }
        public UserCollection() => CollectionFeeds = new List<CollectionFeeds>();
    }
}
