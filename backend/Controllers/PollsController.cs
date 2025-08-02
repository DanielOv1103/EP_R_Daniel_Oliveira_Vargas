// Controllers/PollsController.cs
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
    [Authorize]                        // GET requires login
    public class PollsController : ControllerBase
    {
        private readonly VotingDbContext _db;
        public PollsController(VotingDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _db.Polls.Where(p => p.Status == Status.Active).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _db.Polls
                             .Include(x => x.Options)
                             .SingleOrDefaultAsync(x => x.Id == id && x.Status == Status.Active);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpPost, Authorize(Roles="Admin")]
        public async Task<IActionResult> Create(CreatePollDto dto)
        {
            var p = new Poll { Title = dto.Title, Description = dto.Description };
            _db.Polls.Add(p);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = p.Id }, p);
        }

        [HttpPut("{id}"), Authorize(Roles="Admin")]
        public async Task<IActionResult> Update(int id, UpdatePollDto dto)
        {
            var p = await _db.Polls.FindAsync(id);
            if (p == null) return NotFound();
            p.Title     = dto.Title;
            p.Description = dto.Description;
            p.IsActive  = dto.IsActive;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}"), Authorize(Roles="Admin")]
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
