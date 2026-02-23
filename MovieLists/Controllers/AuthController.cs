using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace MovieLists.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDto userDto)
        {
            try
            {
                var token = await _authService.RegisterAsync(userDto);
                return Ok(new { token, message = "Kayıt başarılı!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
                return Ok(new { token = result.Token, username = result.Username, email = result.Email, message = "Giriş başarılı!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("~/Account/Login")]
        [AllowAnonymous]
        public IActionResult LoginView()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        [HttpGet]
        [Route("~/Account/Profile")]
        public IActionResult ProfileView()
        {
            return View("~/Views/Account/Profile.cshtml");
        }
    }
}