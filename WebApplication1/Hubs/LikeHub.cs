using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.Providers;

namespace WebApplication1.Hubs
{
    public class LikeHub : Hub
    {
        private readonly IMsgProvider _msgProvider;

        public LikeHub(IMsgProvider msgProvider)
        {
            _msgProvider = msgProvider;
        }
        public async Task SendLike (string id, string email)

        {
            _msgProvider.AddLike(id,email);
            await Clients.All.SendAsync("SendLike",id);
        }

        public async Task DisLike(string id,string email)
        {
            _msgProvider.RemoveLike(id,email);
            await Clients.All.SendAsync("DisLike", id);
        }
    }
}
