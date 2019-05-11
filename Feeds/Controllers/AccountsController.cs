using System.Linq;
using Feeds.Controllers.Models;
using Feeds.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Feeds.Controllers
{
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IUserRepository _userRepository;

        public AccountsController(ITokenGenerator tokenGenerator, 
                                  IUserRepository userRepository)
        {
            _tokenGenerator = tokenGenerator;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<string> Register([FromBody] RegistrationRequest registrationRequest)
        {
            if (_userRepository.Save(
                new User
                {
                    Name = registrationRequest.Name,
                    Email = registrationRequest.Email,
                    PasswordHash = registrationRequest.PasswordHash
                }) == true)
            {
                return Ok(_tokenGenerator.Generate(registrationRequest));
            }
            return BadRequest();
        }

        [HttpPost]
        public ActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var claimList = HttpContext.User.Claims.ToList();
            if (claimList.Where(x => x.Value == loginRequest.Email).SingleOrDefault() != null)
            {
                User UserData = _userRepository.FindByEmail(loginRequest.Email);
                if (UserData.PasswordHash == loginRequest.PasswordHash)
                {
                   return Ok();
                }
            }
            return Unauthorized();
        }
    }
}
