using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.Dto;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IClientRepository _clientRepository;
        private IPasswordHasher _passwordHasher;
        public AuthController(IClientRepository clientRepository, IPasswordHasher passwordHasher)
        {
            _clientRepository = clientRepository;
            _passwordHasher = passwordHasher;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] NewClientDto NewClientDto)
        {
            try
            {
                Client user = _clientRepository.FindByEmail(NewClientDto.Email);

                var result = _passwordHasher.VerifyPassword(user.HashedPassword, NewClientDto.Password);
                if (result == false)
                {
                    throw new Exception("Username is not correct");
                }
                
                //if (user == null || !String.Equals(user.HashedPassword, client.HashedPassword))
                
                //    return Unauthorized();

                var claims = new List<Claim>
                {
                    new Claim("Client", user.Email),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                    );

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {

                await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
