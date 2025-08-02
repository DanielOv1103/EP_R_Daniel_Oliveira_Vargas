// Controllers/VotesController.cs
using System.Linq;
using System.Security.Claims;
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
    [Authorize]
    public class VotesController : ControllerBase
    {
        private readonly VotingDbContext _db;
        public VotesController(VotingDbContext db) => _db = db;

        [HttpPost]
        public async Task<IActionResult> Vote(CreateVoteDto dto)
        {
            var uid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (await _db.Votes.AnyAsync(v => v.UserId == uid && v.PollId == dto.PollId))
                return BadRequest("Ya votaste esta encuesta.");

            var v = new Vote {
                UserId   = uid,
                PollId   = dto.PollId,
                OptionId = dto.OptionId
            };
            _db.Votes.Add(v);
            await _db.SaveChangesAsync();
            return Ok(v);
        }

        [HttpGet("results/{pollId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Results(int pollId)
        {
            var res = await _db.Votes
                .Where(v => v.PollId == pollId && v.Status == VoteStatus.Active)
                .GroupBy(v => v.OptionId)
                .Select(g => new { OptionId = g.Key, Count = g.Count() })
                .ToListAsync();
            return Ok(res);
        }
    }
}
