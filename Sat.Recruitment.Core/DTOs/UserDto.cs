using Sat.Recruitment.Core.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Core.DTOs
{
    public class UserDto
    {
        private const string EmailFormatMessage = "Email address wrong format. Considere this ex. username@domain.com";

        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be empty")]
        public string Name { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = EmailFormatMessage)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email cannot be empty")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Address cannot be empty")]
        public string Address { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Phone cannot be empty")]
        [MaxLength(13)]
        public string Phone { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserType cannot be empty")]
        public int UserTypeId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Money cannot be empty")]
        public decimal Money { get; set; }
    }
}
