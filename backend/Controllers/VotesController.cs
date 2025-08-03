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
    [Route("api/votes")]
    [Authorize]
    public class VotesController : ControllerBase
    {
        private readonly VotingDbContext _db;
        public VotesController(VotingDbContext db) => _db = db;

        // POST /api/polls/{pollId}/votes
        [HttpPost]
        public async Task<IActionResult> Vote(int pollId, CreateVoteDto dto)
        {
            // 1️⃣ Asegurarse de que la encuesta existe y está activa
            var pollExists = await _db.Polls
                .AnyAsync(p => p.Id == pollId && p.Status == Status.Active);
            if (!pollExists)
                return NotFound(new { message = "Encuesta no encontrada." });

            // 2️⃣ Obtener el UserId desde el token
            var uidClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(uidClaim, out var uid))
                return Unauthorized();

            // 3️⃣ Verificar que no haya votado ya
            var already = await _db.Votes
                .AnyAsync(v => v.UserId == uid && v.PollId == pollId && v.Status == VoteStatus.Active);
            if (already)
                return BadRequest(new { message = "Ya votaste esta encuesta." });

            // 4️⃣ Crear y guardar el voto
            var vote = new Vote
            {
                UserId = uid,
                PollId = pollId,
                OptionId = dto.OptionId,
                Status = VoteStatus.Active
            };
            _db.Votes.Add(vote);
            await _db.SaveChangesAsync();

            // 5️⃣ Devolver 201 Created con la ruta al nuevo voto
            return CreatedAtAction(
                nameof(Vote),
                new { pollId = pollId },
                new
                {
                    id = vote.Id,
                    pollId = vote.PollId,
                    optionId = vote.OptionId,
                    userId = vote.UserId
                }
            );
        }

        // GET /api/polls/{pollId}/votes/results
        [HttpGet("results")]
        [AllowAnonymous]
        public async Task<IActionResult> Results(int pollId)
        {
            var counts = await _db.Votes
                .Where(v => v.PollId == pollId && v.Status == VoteStatus.Active)
                .GroupBy(v => v.OptionId)
                .Select(g => new { OptionId = g.Key, Count = g.Count() })
                .ToListAsync();

            return Ok(counts);
        }

        /// <summary>
        /// GET /api/votes/me
        /// Lista todas las votaciones del usuario actual,
        /// con nombre de encuesta y texto de la opción.
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> MyVotes()
        {
            // 1️⃣ Extrae userId del token
            var uidClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(uidClaim, out var uid))
                return Unauthorized();

            // 2️⃣ Consulta los votos activos de este usuario
            var votes = await _db.Votes
                .Where(v => v.UserId == uid && v.Status == VoteStatus.Active)
                .Include(v => v.Poll)
                .Include(v => v.Option)
                .ToListAsync();

            // 3️⃣ Proyecta solo los datos que quieres exponer
            var result = votes.Select(v => new
            {
                voteId = v.Id,
                pollId = v.PollId,
                pollTitle = v.Poll.Title,
                optionId = v.OptionId,
                optionText = v.Option.Text,
                votedAt = v.CreatedAt  // si tu entidad Vote lleva timestamp
            });

            return Ok(result);
        }
    }
}
