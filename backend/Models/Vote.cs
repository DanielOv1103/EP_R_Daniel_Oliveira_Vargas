// Models/Vote.cs
using System.ComponentModel.DataAnnotations;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Models
{
    public enum VoteStatus { Active, Pending, Counted, Rejected }

    public class Vote
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int PollId { get; set; }
        public Poll Poll { get; set; }

        [Required]
        public int OptionId { get; set; }
        public Option Option { get; set; }

        public VoteStatus Status { get; set; } = VoteStatus.Active;

        public DateTime CreatedAt { get; set; }
        
        public Vote()
        {
            // Inicializa CreatedAt al construir la entidad
            CreatedAt = DateTime.UtcNow;
        }
    }
}
