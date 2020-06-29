using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication1.Models.ListaMessaggi;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       
        private readonly IMsgProvider _msgProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IMsgProvider msgProvider, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _msgProvider = msgProvider;
        }


        public IActionResult Index()
        {
            var z1 =User.Claims.Where(x => x.Type == "email").Select(k => k.Value).First();
            var z2 = User.Claims.Where(x => x.Type == "nome").Select(k => k.Value).First();
            var z3 = User.Claims.Where(x => x.Type == "ruolo").Select(k => k.Value).First();
            z3 = (z3 == "1") ? "Guest" : "Admin";
            _httpContextAccessor.HttpContext.Session.SetString("nome", z2);
            _httpContextAccessor.HttpContext.Session.SetString("email", z1);
            _httpContextAccessor.HttpContext.Session.SetString("ruolo", z3);
            //var userEmail = _httpContextAccessor.HttpContext.Session.GetString("email");
            var listMsgUser = _msgProvider.GetAllMessage(z1);
            ListaMessaggiModel model = new ListaMessaggiModel();
            model.ListMessage = listMsgUser;
            model.Email = z1;
            return View(model);
        }

        
    }
}
