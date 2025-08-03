using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.EP_R_Daniel_Oliveira_Vargas.Data;
using backend.EP_R_Daniel_Oliveira_Vargas.DTOs;
using backend.EP_R_Daniel_Oliveira_Vargas.Models;
using System.Security.Claims;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Controllers
{
    [ApiController]
    [Route("api/votes")]
    [Authorize]
    public class VotesController : ControllerBase
    {
        private readonly VotingDbContext _db;
        public VotesController(VotingDbContext db) => _db = db;

        [HttpPost]
        public async Task<IActionResult> Vote(int pollId, CreateVoteDto dto)
        {
            var pollExists = await _db.Polls
                .AnyAsync(p => p.Id == pollId && p.Status == Status.Active);
            if (!pollExists)
                return NotFound(new { message = "Encuesta no encontrada." });

            var uidClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(uidClaim, out var uid))
                return Unauthorized();

            var already = await _db.Votes
                .AnyAsync(v => v.UserId == uid && v.PollId == pollId && v.Status == VoteStatus.Active);
            if (already)
                return BadRequest(new { message = "Ya votaste esta encuesta." });

            var vote = new Vote
            {
                UserId = uid,
                PollId = pollId,
                OptionId = dto.OptionId,
                Status = VoteStatus.Active
            };
            _db.Votes.Add(vote);
            await _db.SaveChangesAsync();

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

        [HttpGet("results"), AllowAnonymous]
        public async Task<IActionResult> Results(int pollId)
        {
            var counts = await _db.Votes
                .Where(v => v.PollId == pollId && v.Status == VoteStatus.Active)
                .GroupBy(v => v.OptionId)
                .Select(g => new { OptionId = g.Key, Count = g.Count() })
                .ToListAsync();

            return Ok(counts);
        }

        [HttpGet("me")]
        public async Task<IActionResult> MyVotes()
        {
            var uidClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(uidClaim, out var uid))
                return Unauthorized();

            var votes = await _db.Votes
                .Where(v => v.UserId == uid && v.Status == VoteStatus.Active)
                .Include(v => v.Poll)
                .Include(v => v.Option)
                .ToListAsync();

            var result = votes.Select(v => new
            {
                voteId = v.Id,
                pollId = v.PollId,
                pollTitle = v.Poll.Title,
                optionId = v.OptionId,
                optionText = v.Option.Text,
                votedAt = v.CreatedAt
            });

            return Ok(result);
        }

        [HttpGet("results/global"), AllowAnonymous]
        public async Task<IActionResult> GetGlobalResults()
        {
            var stats = await _db.Polls
                .Where(p => p.Status == Status.Active)
                .Select(p => new
                {
                    id = p.Id,
                    title = p.Title,
                    totalVotes = p.Options.SelectMany(o => o.Votes).Count()
                })
                .OrderByDescending(p => p.totalVotes)
                .ToListAsync();

            return Ok(stats);
        }

        [HttpGet("results/{pollId}"), AllowAnonymous]
        public async Task<IActionResult> GetPollResults(int pollId)
        {
            var poll = await _db.Polls
                .Where(p => p.Id == pollId && p.Status == Status.Active)
                .Select(p => new
                {
                    id = p.Id,
                    title = p.Title,
                    options = p.Options
                        .Select(o => new
                        {
                            id = o.Id,
                            text = o.Text,
                            votes = o.Votes.Count()
                        })
                        .OrderByDescending(o => o.votes)
                        .ToList()
                })
                .SingleOrDefaultAsync();

            if (poll == null) return NotFound();
            return Ok(poll);
        }

        [HttpGet("results/{pollId}/pdf"), AllowAnonymous]
        public async Task<IActionResult> ExportPollResultsToPdf(int pollId)
        {
            var poll = await _db.Polls
                .Where(p => p.Id == pollId && p.Status == Status.Active)
                .Select(p => new
                {
                    title = p.Title,
                    options = p.Options.Select(o => new
                    {
                        text = o.Text,
                        votes = o.Votes.Count()
                    }).OrderByDescending(o => o.votes).ToList()
                })
                .SingleOrDefaultAsync();

            if (poll == null) return NotFound();

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text($"Resultados de la encuesta").SemiBold().FontSize(16).AlignCenter();
                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Título: {poll.title}").Bold();
                        col.Item().Text($"Fecha: {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Opción").Bold();

                                header.Cell().Element(CellStyle).Text("Votos").Bold();
        
                            });

                            foreach (var opt in poll.options)
                            {
                                table.Cell().Element(CellStyle).Text(opt.text);
                                table.Cell().Element(CellStyle).Text(opt.votes.ToString());
                            }

                            static IContainer CellStyle(IContainer container)
                                => container.PaddingVertical(5).PaddingHorizontal(5);
                        });
                    });

                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Generado automáticamente - JaguarExpress").FontSize(10);
                    });
                });
            });

            var stream = new MemoryStream();
            pdf.GeneratePdf(stream);
            stream.Position = 0;

            return File(stream, "application/pdf", $"Resultados_Encuesta_{pollId}.pdf");
        }
    }
}
