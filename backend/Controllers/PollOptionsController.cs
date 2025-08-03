using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.EP_R_Daniel_Oliveira_Vargas.Data;
using backend.EP_R_Daniel_Oliveira_Vargas.Models;
using backend.EP_R_Daniel_Oliveira_Vargas.DTOs;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Controllers
{
    [ApiController]
    [Route("api/Polls/{pollId}/Options")]
    public class PollOptionsController : ControllerBase
    {
        private readonly VotingDbContext _db;
        public PollOptionsController(VotingDbContext db) => _db = db;

        // DELETE /api/Polls/{pollId}/Options/{optionId}
        [HttpDelete("{optionId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOption(int pollId, int optionId)
        {
            var option = await _db.Options.FindAsync(optionId);
            if (option == null || option.PollId != pollId)
            {
                return NotFound();
            }

            _db.Options.Remove(option);  // ✅ Eliminación real
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{optionId}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOption(int pollId, int optionId, UpdateOptionDto dto)
        {
            var option = await _db.Options.FindAsync(optionId);
            if (option == null || option.PollId != pollId)
            {
                return NotFound();
            }

            option.Text = dto.Text;  // ✅ Actualiza el texto
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

