
using IdentityModel;
using System.Security.Claims;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

namespace WebApplication.DataAccess.SQL.DataModels
{
    public class Utente 
    {
        [Key]
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Password { get; set; }
        public int Ruolo { get; set; }

        [DefaultValue("~/img/profile-photos/1.png")]
        public string Img { get; set; }
    }
}
