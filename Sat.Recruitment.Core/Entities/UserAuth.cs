using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Core.Entities
{
    public class UserAuth : BaseEntity
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
