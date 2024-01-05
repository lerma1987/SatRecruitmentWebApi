using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Core.Entities
{
    public class UserDetails : BaseEntity
    {
        [Key]
        public int UserDetailId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int UserTypeId { get; set; }
        public string AppUserId { get; set; }        
        public decimal Money { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
