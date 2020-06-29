using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApplication.DataAccess.SQL.DataModels
{
    public class Ruolo
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
