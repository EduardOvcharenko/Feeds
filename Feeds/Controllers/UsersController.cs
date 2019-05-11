using System.Collections.Generic;
using Feeds.Models;
using Microsoft.AspNetCore.Mvc;

namespace Feeds.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return Ok(_userRepository.GetUsers());
        }
    }
}