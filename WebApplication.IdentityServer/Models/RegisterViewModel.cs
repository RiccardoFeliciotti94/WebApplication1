using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.IdentityServer.Models
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }        
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Le password non corrispondono.")]
        public string Password2 { get; set; }
        public string ReturnUrl { get; set; }
    }
}
