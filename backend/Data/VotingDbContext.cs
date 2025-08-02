// Data/VotingDbContext.cs
using Microsoft.EntityFrameworkCore;
using backend.EP_R_Daniel_Oliveira_Vargas.Models;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Data
{
    public class VotingDbContext : DbContext
    {
        public VotingDbContext(DbContextOptions<VotingDbContext> opts)
            : base(opts) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<AuthToken>  AuthTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder b)
        {
            // Única votación por usuario/poll
            b.Entity<Vote>()
             .HasIndex(v => new { v.UserId, v.PollId })
             .IsUnique();

            base.OnModelCreating(b);
        }
    }
}
