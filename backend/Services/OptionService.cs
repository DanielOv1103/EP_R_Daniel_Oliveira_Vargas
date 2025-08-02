using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.EP_R_Daniel_Oliveira_Vargas.DTOs;
using backend.EP_R_Daniel_Oliveira_Vargas.Data;
using backend.EP_R_Daniel_Oliveira_Vargas.Models;
using backend.EP_R_Daniel_Oliveira_Vargas.Controllers; // DTO: CreateOptionDto

namespace backend.EP_R_Daniel_Oliveira_Vargas.Services
{
    public interface IOptionService
    {
        Task<Option> AddOptionAsync(int pollId, CreateOptionDto dto);
    }

    public class OptionService : IOptionService
    {
        private readonly VotingDbContext _context;
        public OptionService(VotingDbContext context) => _context = context;

        public async Task<Option> AddOptionAsync(int pollId, CreateOptionDto dto)
        {
            if (!await _context.Polls.AnyAsync(p => p.Id == pollId && p.Status == Status.Active))
                throw new KeyNotFoundException("Poll not found.");

            var option = new Option
            {
                PollId = pollId,
                Text = dto.Text,
                Status = Status.Active
            };
            _context.Options.Add(option);
            await _context.SaveChangesAsync();
            return option;
        }
    }
}
