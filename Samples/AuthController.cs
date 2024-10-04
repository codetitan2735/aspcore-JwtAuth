/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Onesoftdev.AspCoreJwtAuth.Auth;
using Onesoftdev.AspCoreJwtAuth.Services;

namespace Onesoftdev.WebApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("login")]
        [HttpPost(Name = "login")]
        public async Task<IActionResult> Login([FromBody]UserLogin userLogin)
        {
            var authToken = await _userService.Authenticate(userLogin);

            if (authToken == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(authToken);
        }
    }
}
 */
