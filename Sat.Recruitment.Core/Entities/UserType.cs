
namespace Sat.Recruitment.Core.Entities
{
    public class UserType : BaseEntity
    {
        public UserType()
        {
            Users = new HashSet<User>();
        }
        public string TypeName { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
