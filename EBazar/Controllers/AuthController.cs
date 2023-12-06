using EBazar_BAL.Interfaces;
using EBazar_BAL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EBazar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly IEmailSender _emailSender;
        private readonly IUserManager _userManager;
        private static Random random = new Random();
        private readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private string key;
        public AuthController(IAuthManager authManager, IEmailSender emailSender, IUserManager userManager)
        {
            _authManager = authManager;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            key = RandomString(8);
            var result = await _authManager.Register(model, key);
            switch (result)
            {
                case 1:
                    {
                        var email = model.Email;
                        var subject = "Welcome";
                        var message = "Thank you for your registration. Use this code to activate your email: ";
                        message += key;

                        await _emailSender.SendEmailAsync(email, subject, message);
                        return Ok("Succes!");
                    }
                case 2:
                    return Ok("Email already used");
                case 3:
                    return Ok("Username already used");
                case 0:
                    return BadRequest("Error at register");
                default:
                    return BadRequest("Error at register");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await _authManager.Login(model);

            return Ok(result);
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshModel model)
        {
            var result = await _authManager.Refresh(model);

            return Ok(result);
        }

        [HttpGet("activateEmail")]
        public async Task<IActionResult> ConfirmEmail(string key, string email)
        {

            var result = await _userManager.ConfirmEmail(email, key);
            if (result == true)
            {
                return Ok("Success");
            }
            return BadRequest("Error");
        }

        [HttpPost]
        [Route("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var result = await _authManager.ChangePassword(model);
            if (!result)
            {
                return BadRequest("Could not change password");
            }

            return Ok("Password changed successfully");
        }
    }
}
