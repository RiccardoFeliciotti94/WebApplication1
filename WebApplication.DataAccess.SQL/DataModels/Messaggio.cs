using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApplication.DataAccess.SQL.DataModels
{
    public class Messaggio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string IDMessaggio { get; set; }
        public string Testo { get; set; }
        public string Data { get; set; }
        public string Email { get; set; }

        public int NLike { get; set; }

    }
}
