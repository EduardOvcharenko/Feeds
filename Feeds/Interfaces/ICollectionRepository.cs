using Feeds.ControllerModels;
using Feeds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds
{
    public interface ICollectionRepository
    {
        List<UserCollection> GetUserCollections(Guid UserId);
        List<UserCollection> GetCollections();

        bool Create(UserCollection createRequest);
        bool Rename(UserCollection renameRequest);
        bool Delete(Guid CollectionId);

        bool AddFeedToCollection(CollectionFeeds collectionFeed);
        bool DeleteFeedFromCollection(CollectionFeeds collectionFeed);
    }
}
