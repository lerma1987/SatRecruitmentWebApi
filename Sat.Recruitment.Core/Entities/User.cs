using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Core.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int UserTypeId { get; set; }
        public virtual UserType UserType { get; set; }
        public decimal Money { get; set; }
    }
}
