using Feeds.Models;
using System.Collections.Generic;

namespace Feeds
{
    public interface IUserRepository
    {
        bool Save(User user);
        User FindByEmail(string email);
        List<User> GetUsers();
    }
}