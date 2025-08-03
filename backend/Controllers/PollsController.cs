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
        // GET /api/Polls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var polls = await _db.Polls
                .Where(p => p.Status == Status.Active)
                .Include(p => p.Options)
                .Select(p => new
                {
                    id = p.Id,
                    title = p.Title,
                    description = p.Description,
                    isActive = p.Status == Status.Active,
                    options = p.Options.Select(o => new
                    {
                        id = o.Id,
                        text = o.Text
                    }).ToList()
                })
                .ToListAsync();

            return Ok(polls);
        }

        // GET /api/Polls/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var poll = await _db.Polls
                .Include(p => p.Options)
                .Where(p => p.Id == id && p.Status == Status.Active)
                .Select(p => new
                {
                    id,
                    title = p.Title,
                    description = p.Description,
                    isActive = p.Status == Status.Active,
                    options = p.Options.Select(o => new
                    {
                        id = o.Id,
                        text = o.Text
                    }).ToList()
                })
                .SingleOrDefaultAsync();

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
        [HttpPatch("{id}"), Authorize(Roles = "Admin")]
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
