using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParksAPI.Models;
using ParksAPI.Repository.IRepository;

namespace ParksAPI.Controllers
{
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserAuthentiication user)
        {
            var result = _userRepo.Authenticate(user.Username, user.Password);
            if (result == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(result);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegister user)
        {
            bool uniqueName = _userRepo.IsUniqueUser(user.Username);

            if (uniqueName)
            {
                var userResult = _userRepo.Register(user.Username, user.Password);
                
                if (userResult == null)
                    return BadRequest(new { message = "Error while registering" });

                return Ok();
            }

            return BadRequest(new { message = "Username already exists" });
        }
    }
}
