using Microsoft.AspNetCore.Identity;

namespace NoumanJobPortal.Models.Users
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsBanned { get; set; } = false;
    }
}
