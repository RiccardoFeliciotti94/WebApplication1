using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication.DataAccess.SQL.DataModels
{
    public class CommentoModel
    {
        public string Email { get; set; }

        public string Nome { get; set; }

        public string IDMessaggio { get; set; }

        public string Img { get; set; }

        public string TestoCommento { get; set; }

        public DateTime Data { get; set; }
    }
}
