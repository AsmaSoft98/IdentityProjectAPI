using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace identityProjectAPI.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {

        }
        [Required(ErrorMessage = "First name is required!")]
        [StringLength(60)]
        public string FirstName { get; set; }
      
        [Required(ErrorMessage = "Last name is required!")]
        [StringLength(60)] 
        public string LastName { get; set; }

    }
}
