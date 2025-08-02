// Models/AuthToken.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Models
{
    public class AuthToken
    {
        public int Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
