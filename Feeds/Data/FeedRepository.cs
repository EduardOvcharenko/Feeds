using Feeds.ControllerModels;
using Feeds.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feeds.Data
{
    public class FeedRepository : IFeedRepository
    {
        private readonly IMemoryCache _cache;
        private readonly ApplicationDbContext _context;
        private readonly ICacheRefresher _cacheRefresher;

        public FeedRepository(ApplicationDbContext context, IMemoryCache cache, ICacheRefresher cacheRefresher)
        {
            _context = context ?? throw new NullReferenceException();

            _cache = cache;
            _cacheRefresher = cacheRefresher;
            if (!_cache.TryGetValue("Feeds", out List<Feed> feedsCollections))
            {
                var  a = _cache.Get("Feeds");
                _cacheRefresher.Refresh();
            }
        }

        private Feed FindById(Guid feedId)
        {
            return _context.Feeds
                .Include(p=>p.Posts)
                .Where(feed => feed.Id == feedId).SingleOrDefault();
        }

        public List<Feed> GetAllFeeds()
        {
            //return _context.Feeds.Include(p => p.Posts).ToList();
            _cache.TryGetValue("Feeds", out List<Feed> feedsCollections);
            if (feedsCollections != null)
            {
                return feedsCollections;
            }
            return null;

        }

        public bool Create(Feed feed)
        {
            if (_context.Add(feed) != null)
            {
                _context.SaveChanges();
                _cacheRefresher.Refresh();
                return true;
            }
            return false;
        }

        public bool Delete(Guid feedId)
        {
            if (_context.Remove(FindById(feedId)) != null)
            {
                _context.SaveChanges();
                _cacheRefresher.Refresh();
                return true;
            }
            return false;
        }

        public bool AddPosts(Post post)
        {
            _context.Add(post);
            _context.SaveChanges();
            return true;
        }


    }
}
