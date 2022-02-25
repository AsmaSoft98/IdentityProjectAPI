using System;
using System.ComponentModel.DataAnnotations;

namespace SharedProject.Requests
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
