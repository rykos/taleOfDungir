using System.ComponentModel.DataAnnotations;

namespace taleOfDungir.Models
{
    public class LoginModel
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Username is too long")]
        [MinLength(4, ErrorMessage = "Username is too short")]
        public string Username { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Username is too long" )]
        [MinLength(8, ErrorMessage = "Username is too short")]
        public string Password { get; set; }
    }
}