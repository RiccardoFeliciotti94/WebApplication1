using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApplication.DataAccess.SQL.DataModels
{
    public class Utente
    {
        [Key]
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Password { get; set; }

        public int Ruolo { get; set; }
    }
}
