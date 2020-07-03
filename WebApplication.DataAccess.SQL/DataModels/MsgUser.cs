using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace WebApplication.DataAccess.SQL.DataModels
{
    public class MsgUser
    {
        public string IDMessaggio { get; set; }
        public string Testo { get; set; }
        public string Nome { get; set; }
        public string Img { get; set; }
        public string Data { get; set; }
        public int Like { get; set; }
        public int SetLike { get; set; }

        public List<CommentoModel> Commenti { get; set; }
    }
}
