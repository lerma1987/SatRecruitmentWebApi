using Sat.Recruitment.Core.Entities;

namespace Sat.Recruitment.Core.DTOs
{
    public class UserLoginResponseDto
    {
        public UserAuth Usuario { get; set; }
        public string Token { get; set; }
    }
}