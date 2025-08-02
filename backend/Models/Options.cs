// Models/Option.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.EP_R_Daniel_Oliveira_Vargas.Models
{
    public class Option
    {
        public int Id { get; set; }

        [Required]
        public int PollId { get; set; }
        public Poll Poll    { get; set; }

        [Required, StringLength(500)]
        public string Text  { get; set; }

        public Status Status { get; set; } = Status.Active;

        public ICollection<Vote> Votes { get; set; }
    }
}
