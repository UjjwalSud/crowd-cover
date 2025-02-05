using System.ComponentModel.DataAnnotations;

namespace CrowdCover.Web.Models
{
  
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        //public string ConfirmPassword { get; set; }        

        [Required]
        public string Username { get; set; }       
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
