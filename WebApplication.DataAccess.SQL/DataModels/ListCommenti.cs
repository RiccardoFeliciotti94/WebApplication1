using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication.DataAccess.SQL.DataModels
{
    public class ListCommenti
    {
        public Commento Commento { get; set; }

        public List<Commento> SubCommenti { get; set; }
    }
}
