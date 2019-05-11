using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds.Models
{
    public class CollectionFeeds
    {
        [ForeignKey(nameof(Feed))]
        public Guid FeedId { get; set; }
        public Feed Feed { get; set; }

        [ForeignKey(nameof(Collection))]
        public Guid CollectionId { get; set; }
        public UserCollection Collection { get; set; }
    }
}
