using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication1.Models.DataModel;
using WebApplication1.Models.ListaMessaggi;
using WebApplication1.Helper;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Hubs;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       
        private readonly IMsgProvider _msgProvider;
        private readonly ICommentiProvider _commentiProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMsgUserHelper _msgUserHelper;
        private readonly IHubContext<CommentoHub> _hubContext;

        public HomeController(
            IMsgProvider msgProvider, 
            IHttpContextAccessor httpContextAccessor, 
            ICommentiProvider commentiProvider,
            IMsgUserHelper msgUserHelper,
            IHubContext<CommentoHub> hubContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _msgProvider = msgProvider;
            _commentiProvider = commentiProvider;
            _msgUserHelper = msgUserHelper;
            _hubContext = hubContext;
        }


        public IActionResult Index()
        {
            var emailSession = _httpContextAccessor.HttpContext.Session.GetString("email");

            if (emailSession == null)
            {
                var emailClaim = User.Claims.Where(x => x.Type == "email").Select(k => k.Value).First();
                var nameClaim = User.Claims.Where(x => x.Type == "nome").Select(k => k.Value).First();
                var roleClaim = User.Claims.Where(x => x.Type == "ruolo").Select(k => k.Value).First();
                roleClaim = (roleClaim == "1") ? "Guest" : "Admin";
                var imgClaim = User.Claims.Where(x => x.Type == "immagine").Select(k => k.Value).First();
                _httpContextAccessor.HttpContext.Session.SetString("nome", nameClaim);
                _httpContextAccessor.HttpContext.Session.SetString("email", emailClaim);
                _httpContextAccessor.HttpContext.Session.SetString("ruolo", roleClaim);
                _httpContextAccessor.HttpContext.Session.SetString("immagine", imgClaim);


                emailSession = emailClaim;
            }


            var listMsgUser = _msgUserHelper.GetMessaggi(emailSession);
            ListaMessaggiModel model = new ListaMessaggiModel
            {
                ListMessage = listMsgUser,
                Email = emailSession
            };
            return View(model);
        }

        public IActionResult Logout ()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            return SignOut("Cookies", "oidc");
        }

        public IActionResult PostCommento (ListaMessaggiModel model)
        {
            _commentiProvider.AddCommento(model.Testo,model.Email,model.IDMes);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> PostSubCommento (ListaMessaggiModel model)
        {
            _commentiProvider.AddCommento(model.Testo, model.Email, model.IDMes,model.IDRefCom);
            await _hubContext.Clients.All.SendAsync("SendCommento", 
                DateTime.Now.ToString("dd/MM/yyyy hh:mm"),
                _httpContextAccessor.HttpContext.Session.GetString("nome"),
                model.Testo, 
                _httpContextAccessor.HttpContext.Session.GetString("immagine"),
                model.IDRefCom);
            // await _hubContext.Clients.AllExcept
            return RedirectToAction("Index", "Home");
        }
        
    }
}
