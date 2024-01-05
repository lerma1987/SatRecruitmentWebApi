using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Sat.Recruitment.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string Fullname { get; set; }
        public virtual UserDetails UserDetails { get; set; } //Reference navigation to dependent
    }
}