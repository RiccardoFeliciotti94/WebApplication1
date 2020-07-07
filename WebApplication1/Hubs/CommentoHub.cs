using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.Providers;

namespace WebApplication1.Hubs
{
    public class CommentoHub : Hub
    {
        private readonly ICommentiProvider _commentiProvider;

        public CommentoHub (ICommentiProvider commentiProvider)
        {
            _commentiProvider = commentiProvider;
        }

        public async Task SendCommento(string email,string testo ,string idMes, string idRef)

        {
            _commentiProvider.AddCommento(testo, email, idMes, idRef);
            await Clients.All.SendAsync("SendCommento","", "","","","");
            
        }

    }
}
