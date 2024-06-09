using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Username can not be over 20 characters!")]
        public string? UserName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set;}
        [Required]
        public string? Password { get; set; }
    }
}