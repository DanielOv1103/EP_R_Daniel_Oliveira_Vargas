// Controllers/OptionsController.cs
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
    [Route("api/polls/{pollId}/[controller]")]
    [Authorize(Roles="Admin")]
    public class OptionsController : ControllerBase
    {
        private readonly VotingDbContext _db;
        public OptionsController(VotingDbContext db) => _db = db;

        [HttpPost]
        public async Task<IActionResult> Add(int pollId, CreateOptionDto dto)
        {
            if (!await _db.Polls.AnyAsync(p => p.Id == pollId && p.Status == Status.Active))
                return NotFound("Poll no existe.");

            var o = new Option { PollId = pollId, Text = dto.Text };
            _db.Options.Add(o);
            await _db.SaveChangesAsync();
            return CreatedAtAction(null, new { o.Id }, o);
        }
    }
}
