// Controllers/OptionsController.cs
using System.Collections.Generic;
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
    [Route("api/polls/{pollId}/[controller]")]
    public class OptionsController : ControllerBase
    {
        private readonly VotingDbContext _db;
        public OptionsController(VotingDbContext db) => _db = db;

        // 1️⃣ GET: lista todas las opciones de la encuesta pollId
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int pollId)
        {
            // Verificar que la encuesta exista y esté activa
            var exists = await _db.Polls
                                  .AnyAsync(p => p.Id == pollId && p.Status == Status.Active);
            if (!exists)
                return NotFound(new { message = "Encuesta no encontrada." });

            // Recuperar las opciones
            var opts = await _db.Options
                                .Where(o => o.PollId == pollId)
                                .Select(o => new {
                                    id     = o.Id,
                                    text   = o.Text,
                                    votes  = o.Votes.Count()  // si quisieras contar votos
                                })
                                .ToListAsync();

            return Ok(opts);
        }

        // 2️⃣ POST: crea una nueva opción bajo pollId (solo Admin)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(int pollId, CreateOptionDto dto)
        {
            // Validar existencia de la encuesta
            if (!await _db.Polls.AnyAsync(p => p.Id == pollId && p.Status == Status.Active))
                return NotFound(new { message = "Encuesta no existe." });

            var o = new Option
            {
                PollId = pollId,
                Text   = dto.Text
            };
            _db.Options.Add(o);
            await _db.SaveChangesAsync();

            // Devolver Created 201 con la nueva opción
            return CreatedAtAction(
                nameof(GetAll),
                new { pollId },
                new { id = o.Id, text = o.Text }
            );
        }
    }
}
