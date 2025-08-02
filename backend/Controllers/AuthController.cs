// Controllers/AuthController.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.EP_R_Daniel_Oliveira_Vargas.DTOs;
using backend.EP_R_Daniel_Oliveira_Vargas.Models;
using backend.EP_R_Daniel_Oliveira_Vargas.Services;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _svc;
        public AuthController(IUserService svc) => _svc = svc;

        [HttpPost("register"), ProducesResponseType(201)]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = await _svc.RegisterAsync(dto);
            return CreatedAtAction(null, new { user.Id }, new { user.Id, user.Email });
        }

        [HttpPost("login"), ProducesResponseType(200)]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _svc.AuthenticateAsync(dto);
            return Ok(new { token });
        }
    }
}
