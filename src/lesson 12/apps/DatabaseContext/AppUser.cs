using Microsoft.AspNetCore.Identity;

namespace DatabaseContext
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
