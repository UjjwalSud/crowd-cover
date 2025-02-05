using Microsoft.AspNetCore.Identity;

namespace CrowdCover.Web.Models
{
    public class UserExtra
    {
        public int Id { get; set; } // Id as an int
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Foreign key to IdentityUser
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        //link to Sharpsport 'Bettor'
        public string SharpsportBettorId { get; set; }
    }

}
