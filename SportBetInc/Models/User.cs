using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SportBetInc.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = null!; // id deve ser string e nao um mero numero

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }
    }
}
