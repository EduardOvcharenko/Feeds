using Feeds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Feeds.ControllerModels;
using Microsoft.Extensions.Caching.Memory;

namespace Feeds.Data
{
    public class CollectionRepository: ICollectionRepository
    {
        private readonly IMemoryCache _cache;
        private readonly ICacheRefresher _cacheRefresher;
        private readonly ApplicationDbContext _context;

        public CollectionRepository(ApplicationDbContext context, IMemoryCache cache, ICacheRefresher cacheRefresher)
        {
            _context = context ?? throw new NullReferenceException();

            _cache = cache;
            _cacheRefresher = cacheRefresher;

            if (!_cache.TryGetValue("UserCollections", out List<UserCollection> userCollections))
            {
                _cacheRefresher.Refresh();
            }
        }

        private UserCollection FindById(Guid CollectionId)
        {
            return _context.UserCollections
                            .Include(collection => collection.CollectionFeeds)
                                .ThenInclude(p => p.Feed)
                                .ThenInclude(o => o.Posts)
                            .Where(collection => collection.Id == CollectionId).SingleOrDefault();
        }

        public List<UserCollection> GetUserCollections(Guid UserId)
        {
            _cache.TryGetValue("UserCollections", out List<UserCollection> userCollections);

            if (userCollections != null)
            {
                return userCollections.Where(f => f.UserId == UserId).ToList();
            }
            return null;
        }

        public List<UserCollection> GetCollections()
        {
            _cache.TryGetValue("UserCollections", out List<UserCollection> userCollections);

            if (userCollections != null)
            {
                return userCollections;
            }
            return null;
        }

        public bool Create(UserCollection collection)
        {
            if (_context.Add(collection) != null)
            {
                _context.SaveChanges();
                _cacheRefresher.Refresh();
                return true;
            }
            return false;
        }

        public bool Delete(Guid CollectionId)
        {
            if (_context.Remove(FindById(CollectionId)) != null)
            {
                _context.SaveChanges();
                _cacheRefresher.Refresh();
                return true;
            }
            return false;
        }

        public bool Rename(UserCollection collection)
        {
            //var rename = _context.UserCollections.Where(cf => cf.Id.Equals(collection.Id)).SingleOrDefault();
            var rename = FindById(collection.Id);
            rename.Name = collection.Name;

            if (_context.Update(rename) != null)
            {
                _context.SaveChanges();
                _cacheRefresher.Refresh();
                return true;
            }
            return false;
        }

        public bool AddFeedToCollection(CollectionFeeds collectionFeed)
        {
            var add = _context.CollectionFeeds.Add(collectionFeed);

            _context.SaveChanges();

            _cacheRefresher.Refresh();

            return add != null ? true : false;
        }

        public bool DeleteFeedFromCollection(CollectionFeeds collectionFeed)
        {
            var remove = _context.CollectionFeeds.Where(cf => cf.CollectionId.Equals(collectionFeed.CollectionId)
                                                   && cf.FeedId.Equals(collectionFeed.FeedId));
            _context.Remove(remove);
            _context.SaveChanges();

            _cacheRefresher.Refresh();

            return remove != null ? true : false;
        }


    }
}
