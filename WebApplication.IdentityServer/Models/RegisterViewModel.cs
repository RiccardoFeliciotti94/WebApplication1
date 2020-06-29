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
        public string Email { get; set; }
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Password2 { get; set; }
        public string ReturnUrl { get; set; }


    }
}
