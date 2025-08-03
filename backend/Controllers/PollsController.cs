using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.EP_R_Daniel_Oliveira_Vargas.Data;
using backend.EP_R_Daniel_Oliveira_Vargas.DTOs;
using backend.EP_R_Daniel_Oliveira_Vargas.Models;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]  // opcional: podrías retirarlo y autorizar solo POST/PUT/DELETE
    public class PollsController : ControllerBase
    {
        private readonly VotingDbContext _db;
        public PollsController(VotingDbContext db) => _db = db;

        // 1️⃣ Hacemos pública esta acción
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var polls = await _db.Polls
                                 .Where(p => p.Status == Status.Active)
                                 .ToListAsync();
            return Ok(polls);
        }

        // 2️⃣ Y esta también
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var poll = await _db.Polls
                                .Include(p => p.Options)
                                .SingleOrDefaultAsync(p => p.Id == id && p.Status == Status.Active);
            if (poll == null) return NotFound();
            return Ok(poll);
        }

        // Sólo admins pueden crear
        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreatePollDto dto)
        {
            var p = new Poll
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = Status.Active
            };
            _db.Polls.Add(p);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = p.Id }, p);
        }

        // Sólo admins pueden actualizar
        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, UpdatePollDto dto)
        {
            var p = await _db.Polls.FindAsync(id);
            if (p == null) return NotFound();

            p.Title = dto.Title;
            p.Description = dto.Description;
            // Mapear IsActive a Status
            p.Status = dto.IsActive ? Status.Active : Status.Deleted;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        // Sólo admins pueden "borrar" (marcar soft-delete)
        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Polls.FindAsync(id);
            if (p == null) return NotFound();

            p.Status = Status.Deleted;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
