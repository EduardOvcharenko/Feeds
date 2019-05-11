using Feeds.Controllers.Models;

namespace Feeds
{
    public interface ITokenGenerator
    {
        string Generate(RegistrationRequest registrationRequest);
    }
}
