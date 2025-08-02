// Models/User.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Models
{
    public enum Role   { User = 0, Admin = 1 }
    public enum Status { Active, Deleted, Inactive }

    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public Role Role { get; set; } = Role.User;

        [Required]
        [StringLength(20)]
        public string RoleName { get; set; } = Role.User.ToString();

        [Required]
        public Status Status { get; set; } = Status.Active;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Vote> Votes { get; set; }      = new List<Vote>();
        public ICollection<AuthToken> AuthTokens { get; set; } = new List<AuthToken>();
    }
}
