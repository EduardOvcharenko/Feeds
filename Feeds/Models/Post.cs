using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public Image Image { get; set; }
        public string SourceUrl { get; set; }

        public Guid FeedId { get; set; }
        public Feed Feed { get; set; }
    }
}
