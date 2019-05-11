using Feeds.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Feeds.Data
{
    public class UserRepository  : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new NullReferenceException();
        }

        public List<User> GetUsers()
        {
            return _context.Users
                .Include(uc=>uc.Collections)
                    .ThenInclude(collection => collection.CollectionFeeds)
                    .ThenInclude(p => p.Feed)
                    .ThenInclude(o => o.Posts).ToList();
        }

        public bool Save(User user)
        {
            if (FindByEmail(user.Email) == null)
            {
                _context.Add(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public User FindByEmail(string email)
        {
            return _context.Users
                    .Where(appUser => appUser.Email == email)
                    .SingleOrDefault();
        }
    }
}
