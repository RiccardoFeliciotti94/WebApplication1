using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.DataModels;

namespace WebApplication1.Models.HttpResponse
{
    public class HttpResponseModel
    {
    
            public string Response { get; set; }
            public HttpStatusCode Code { get; set; }

            public string Operation { get; set; }
            public List<Utente> ListUser { get; set; }

           public List<Messaggio> ListMessage { get; set; }
           public string Email { get; set; }
           public string IDMes { get; set; }
           public string NewMessage { get; set; }
           public string NewName { get; set; }
   
           public string Pass { get; set; }
           public int Ruolo { get; set; }
        
    }
}

