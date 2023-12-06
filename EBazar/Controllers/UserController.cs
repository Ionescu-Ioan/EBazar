using EBazar_BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EBazar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }


        [HttpDelete("removeUser/{username}")]
        public async Task<IActionResult> RemoveUser(String username)
        {
            Boolean result = await _userManager.removeUser(username);
            if (result == true)
            {
                return Ok("success");
            }
            else
            {
                return BadRequest("Username does not exist");
            }
        }

        [HttpGet("emailExist")]
        public async Task<IActionResult> EmailExist(String email)
        {
            Boolean result = await _userManager.emailExist(email);
            if (result == false)
            {
                return Ok("false");
            }
            else
            {
                return Ok("true");
            }
        }

        [HttpGet("usernameExist")]
        public async Task<IActionResult> UsernameExist(String username)
        {
            Boolean result = await _userManager.usernameExist(username);
            if (result == false)
            {
                return Ok("false");
            }
            else
            {
                return Ok("true");
            }
        }

        [HttpGet("getUserByUsername")]
        public async Task<IActionResult> getUserByUsername(string username)
        {
            var result = await _userManager.getUserByUsername(username);
            return Ok(result);
        }

    }
}
