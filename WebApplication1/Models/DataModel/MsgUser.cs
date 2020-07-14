using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.DataModel
{
    public class MsgUser
    {
       

        public string IDMessaggio { get; set; }
        public string Testo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Img { get; set; }
        public string Data { get; set; }
        public int Like { get; set; }
        public int SetLike { get; set; }

        public List<CommentoModel> Commenti { get; set; }
    }
}
