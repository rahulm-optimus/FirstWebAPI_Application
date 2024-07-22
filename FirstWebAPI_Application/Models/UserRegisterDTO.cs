using System.ComponentModel.DataAnnotations;

namespace FirstWebAPI_Application.Models
{
    public class UserRegisterDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public  string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public  string Password { get; set; }
        public string[] Roles { get; set; }
    }
}
