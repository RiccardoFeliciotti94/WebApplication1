using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApplication.DataAccess.SQL.DataModels
{
    public class Commento
    {
        [Key]
        public string IDCommento { get; set; }

        public string IDMessaggio { get; set; }

        public string Email { get; set; }        

        public string IDComRef { get; set; }

        public string TestoCommento { get; set; }

        public DateTime Data { get; set; }
    }
}
