using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedProject.Requests
{
    public class ResetPasswordRequest
    {

        //[Required]
        //public string Token { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required] public string CurrentPassword { get; set; }
        [Required] public string NewPassword { get; set; }
        [Required] public string ConfirmNewPassword { get; set; }
    }
}
