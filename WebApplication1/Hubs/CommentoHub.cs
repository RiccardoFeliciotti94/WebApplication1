using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication1.Helper;

namespace WebApplication1.Hubs
{
    public class CommentoHub : Hub
    {
        private readonly ICommentiProvider _commentiProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITimeHelper _timeHelper;

        public CommentoHub (ICommentiProvider commentiProvider, IHttpContextAccessor httpContextAccessor,ITimeHelper timeHelper)
        {
            _commentiProvider = commentiProvider;
            _httpContextAccessor = httpContextAccessor;
            _timeHelper = timeHelper;
        }

        public async Task SendSubCommento(string email, string testo, string idMes, string idref)
        {
            var idCome = _commentiProvider.AddCommento(testo, email, idMes, idref);
            var com = _commentiProvider.GetSingleCommnto(idCome);
            await Clients.All.SendAsync("SendSubCommento", _timeHelper.Converter(com.Data),
                _httpContextAccessor.HttpContext.Session.GetString("nome"),
                testo,
                _httpContextAccessor.HttpContext.Session.GetString("immagine"),
                idref);

        }

        public async Task SendCommento(string email,string testo ,string idMes)
        {
            var idCome = _commentiProvider.AddCommento(testo, email, idMes);
            var com = _commentiProvider.GetSingleCommnto(idCome);
            await Clients.All.SendAsync("SendCommento", idCome, idMes,
                _timeHelper.Converter(com.Data),
                _httpContextAccessor.HttpContext.Session.GetString("nome"),
                email, testo,
                _httpContextAccessor.HttpContext.Session.GetString("immagine"));

        }

    }
}
