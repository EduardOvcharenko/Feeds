using Feeds.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Feeds.Data
{
    public class CacheRefresher : ICacheRefresher
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;

        public CacheRefresher(IMemoryCache cache, ApplicationDbContext context)
        {
            _context = context ?? throw new NullReferenceException();
            _cache = cache;
        }

        List<UserCollection> CollectionRefresh()
        {
            return _context.UserCollections
                    .Include(collectionFeeds => collectionFeeds.CollectionFeeds)
                    .ThenInclude(feeds => feeds.Feed)
                    .ThenInclude(posts => posts.Posts)
                    .ThenInclude(images => images.Image)
                    .ToList();
        }

        List<Feed> FeedsRefresh()
        {
            return _context.Feeds
                    .Include(posts => posts.Posts)
                    .ThenInclude(images => images.Image)
                    .ToList();
        }

        public void Refresh()
        {
            _cache.Set("UserCollections", CollectionRefresh(), new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5), 
                SlidingExpiration = TimeSpan.FromSeconds(5) 
            });
            _cache.Set("Feeds", FeedsRefresh(), new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });
        }
    }
}
