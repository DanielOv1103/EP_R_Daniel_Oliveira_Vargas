// Controllers/UsersController.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.EP_R_Daniel_Oliveira_Vargas.Data;
using backend.EP_R_Daniel_Oliveira_Vargas.DTOs;
using backend.EP_R_Daniel_Oliveira_Vargas.Models;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles="Admin")]
    public class UsersController : ControllerBase
    {
        private readonly VotingDbContext      _db;
        private readonly IPasswordHasher<User> _hasher;

        public UsersController(VotingDbContext db, IPasswordHasher<User> hasher)
        {
            _db     = db;
            _hasher = hasher;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAll()
            => await _db.Users.AsNoTracking().ToListAsync();

        [HttpGet("{id}", Name="GetUser")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return NotFound();
            return Ok(u);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(x => x.Email == dto.Email))
                return Conflict("Email already in use.");
            var u = new User {
                Name     = dto.Name,
                Email    = dto.Email,
                Role     = dto.Role,
                RoleName = dto.Role.ToString(),
                Status   = Status.Active
            };
            u.PasswordHash = _hasher.HashPassword(u, dto.Password);
            _db.Users.Add(u);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetUser", new { id = u.Id }, u);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDto dto)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return NotFound();
            u.Name     = dto.Name;
            u.Email    = dto.Email;
            u.Role     = dto.Role;
            u.RoleName = dto.Role.ToString();
            u.Status   = dto.Status;
            if (!string.IsNullOrWhiteSpace(dto.Password))
                u.PasswordHash = _hasher.HashPassword(u, dto.Password);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return NotFound();
            u.Status = Status.Deleted;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
