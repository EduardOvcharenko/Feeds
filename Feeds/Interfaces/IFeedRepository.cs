using Feeds.ControllerModels;
using Feeds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds
{
    public interface IFeedRepository 
    {
        List<Feed> GetAllFeeds();
        bool Create(Feed createRequest);
        bool Delete(Guid FeedId);
        bool AddPosts(Post postsRequest);
    }
}
