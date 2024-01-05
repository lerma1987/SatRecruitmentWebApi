
namespace Sat.Recruitment.Core.Entities
{
    public class UserType : BaseEntity
    {
        public UserType()
        {
            UserDetails = new HashSet<UserDetails>();
        }
        public string TypeName { get; set; }
        public virtual ICollection<UserDetails> UserDetails { get; set; }
    }
}
