using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.DataModels;

namespace WebApplication1.Models.ListaMessaggi
{
    public class ListaMessaggiModel
    {

        public List<MsgUser> ListMessage {get; set;}
        public string Email { get; set; }

        public string Testo { get; set; }
        public string IDMes { get; set; }

    }
}
