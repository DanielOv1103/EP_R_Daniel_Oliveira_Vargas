using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.EP_R_Daniel_Oliveira_Vargas.DTOs;
using backend.EP_R_Daniel_Oliveira_Vargas.Data;
using backend.EP_R_Daniel_Oliveira_Vargas.Models;
using backend.EP_R_Daniel_Oliveira_Vargas.Controllers; // DTO: CreateVoteDto

namespace backend.EP_R_Daniel_Oliveira_Vargas.Services
{
    public interface IVoteService
    {
        Task<Vote> VoteAsync(int userId, CreateVoteDto dto);
        Task<IEnumerable<object>> GetResultsAsync(int pollId);
    }

    public class VoteService : IVoteService
    {
        private readonly VotingDbContext _context;
        public VoteService(VotingDbContext context) => _context = context;

        public async Task<Vote> VoteAsync(int userId, CreateVoteDto dto)
        {
            if (await _context.Votes.AnyAsync(v => v.UserId == userId && v.PollId == dto.PollId))
                throw new InvalidOperationException("User has already voted.");

            var vote = new Vote
            {
                UserId = userId,
                PollId = dto.PollId,
                OptionId = dto.OptionId,
                Status = VoteStatus.Active
            };
            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();
            return vote;
        }

        public async Task<IEnumerable<object>> GetResultsAsync(int pollId) =>
            await _context.Votes
                .Where(v => v.PollId == pollId && v.Status == VoteStatus.Active)
                .GroupBy(v => v.OptionId)
                .Select(g => new { OptionId = g.Key, Count = g.Count() })
                .ToListAsync();
    }
}
