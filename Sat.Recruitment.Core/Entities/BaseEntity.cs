using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Core.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
