using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Core.DTOs
{
    public class UserRegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
