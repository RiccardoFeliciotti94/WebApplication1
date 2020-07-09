using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.DataModel;

namespace WebApplication1.Models.UtentePanel
{
    public class UtentePanelModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Host { get; set; }
        public string Img { get; set; }
        public List<MsgUser> Messaggi { get; set; }
    }
}
