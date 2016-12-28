using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GolfBag.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public User()
        {

        }
    }
}
