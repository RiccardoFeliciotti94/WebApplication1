using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.DataModel;

namespace WebApplication1.Models.ListaMessaggi
{
    public class ListaMessaggiModel
    {

        public List<MsgUser> ListMessage {get; set;}
        [Required]
        public string Email { get; set; }
    }
}
