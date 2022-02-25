using System.ComponentModel.DataAnnotations;

namespace SharedProject.Requests
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        [StringLength(60)]
        public string Email { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
