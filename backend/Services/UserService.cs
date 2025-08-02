// Services/UserService.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using backend.EP_R_Daniel_Oliveira_Vargas.Data;
using backend.EP_R_Daniel_Oliveira_Vargas.DTOs;
using backend.EP_R_Daniel_Oliveira_Vargas.Models;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(RegisterDto dto);
        Task<string> AuthenticateAsync(LoginDto dto);
    }

    public class UserService : IUserService
    {
        private readonly VotingDbContext      _db;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IJwtService           _jwt;

        public UserService(
            VotingDbContext db,
            IPasswordHasher<User> hasher,
            IJwtService jwt)
        {
            _db     = db;
            _hasher = hasher;
            _jwt    = jwt;
        }

        public async Task<User> RegisterAsync(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                throw new InvalidOperationException("El email ya existe.");

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
            return u;
        }

        public async Task<string> AuthenticateAsync(LoginDto dto)
        {
            var u = await _db.Users.SingleOrDefaultAsync(x => x.Email == dto.Email);
            if (u == null) throw new UnauthorizedAccessException();

            var res = _hasher.VerifyHashedPassword(u, u.PasswordHash, dto.Password);
            if (res != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException();

            return _jwt.GenerateToken(u);
        }
    }
}
