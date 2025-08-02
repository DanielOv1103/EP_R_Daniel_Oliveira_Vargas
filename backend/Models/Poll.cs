// Models/Poll.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Models
{
    public class Poll
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        [Required, StringLength(1000)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
        public Status Status { get; set; } = Status.Active;

        public ICollection<Option> Options { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}
