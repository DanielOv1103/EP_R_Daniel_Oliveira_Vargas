// Controllers/AuthController.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using backend.EP_R_Daniel_Oliveira_Vargas.DTOs;
using backend.EP_R_Daniel_Oliveira_Vargas.Models;
using backend.EP_R_Daniel_Oliveira_Vargas.Services;
using backend.EP_R_Daniel_Oliveira_Vargas.Data;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IUserService _svc;
        private readonly VotingDbContext _db;

        public AuthController(IUserService svc, VotingDbContext db)
        {
            _svc = svc;
            _db = db;                       // <-- lo asignamos
        }

        [HttpPost("register"), ProducesResponseType(201)]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = await _svc.RegisterAsync(dto);
            return CreatedAtAction(null, new { user.Id }, new { user.Id, user.Email });
        }

        [HttpPost("login"), ProducesResponseType(200)]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // 1. Genera el token
            var token = await _svc.AuthenticateAsync(dto);
            if (token == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            // 2. Obtén el usuario desde el DbContext
            var user = await _db.Users
                                .AsNoTracking()
                                .SingleOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return NotFound(new { message = "Usuario no encontrado" });

            // 3. Proyecta solo los campos que quieras exponer
            var resultUser = new
            {
                name = user.Name,
                email = user.Email,
                role = user.Role
            };

            // 4. Devuelve token + datos de usuario
            return Ok(new
            {
                token,
                user = resultUser
            });
        }
    }
}