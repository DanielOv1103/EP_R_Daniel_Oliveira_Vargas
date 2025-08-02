using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.EP_R_Daniel_Oliveira_Vargas.DTOs;
using backend.EP_R_Daniel_Oliveira_Vargas.Data;
using backend.EP_R_Daniel_Oliveira_Vargas.Models;
using backend.EP_R_Daniel_Oliveira_Vargas.Controllers; // DTOs: CreatePollDto, UpdatePollDto

namespace backend.EP_R_Daniel_Oliveira_Vargas.Services
{
    public interface IPollService
    {
        Task<IEnumerable<Poll>> GetAllAsync();
        Task<Poll> GetByIdAsync(int id);
        Task<Poll> CreateAsync(CreatePollDto dto);
        Task UpdateAsync(int id, UpdatePollDto dto);
        Task DeleteAsync(int id);
    }

    public class PollService : IPollService
    {
        private readonly VotingDbContext _context;
        public PollService(VotingDbContext context) => _context = context;

        public async Task<IEnumerable<Poll>> GetAllAsync() =>
            await _context.Polls
                .Where(p => p.Status == Status.Active)
                .ToListAsync();

        public async Task<Poll> GetByIdAsync(int id)
        {
            var poll = await _context.Polls
                .Include(p => p.Options)
                .SingleOrDefaultAsync(p => p.Id == id && p.Status == Status.Active);

            if (poll == null)
                throw new KeyNotFoundException("Poll not found.");

            return poll;
        }

        public async Task<Poll> CreateAsync(CreatePollDto dto)
        {
            var poll = new Poll
            {
                Title = dto.Title,
                Description = dto.Description,
                IsActive = true,
                Status = Status.Active
            };
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();
            return poll;
        }

        public async Task UpdateAsync(int id, UpdatePollDto dto)
        {
            var poll = await _context.Polls.FindAsync(id);
            if (poll == null)
                throw new KeyNotFoundException("Poll not found.");

            poll.Title = dto.Title;
            poll.Description = dto.Description;
            poll.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var poll = await _context.Polls.FindAsync(id);
            if (poll == null)
                throw new KeyNotFoundException("Poll not found.");

            poll.Status = Status.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
