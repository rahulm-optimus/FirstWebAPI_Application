using System.ComponentModel.DataAnnotations;

namespace FirstWebAPI_Application.Models
{
    public class UserDetails
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? City { get; set; }

    }
}
