using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Core.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }
    }
}
