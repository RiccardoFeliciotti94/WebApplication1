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
            var emailSession = _httpContextAccessor.HttpContext.Session.GetString("email");

            if (emailSession == null)
            {
                var emailClaim = User.Claims.Where(x => x.Type == "email").Select(k => k.Value).First();
                var nameClaim = User.Claims.Where(x => x.Type == "nome").Select(k => k.Value).First();
                var roleClaim = User.Claims.Where(x => x.Type == "ruolo").Select(k => k.Value).First();
                roleClaim = (roleClaim == "1") ? "Guest" : "Admin";
                _httpContextAccessor.HttpContext.Session.SetString("nome", nameClaim);
                _httpContextAccessor.HttpContext.Session.SetString("email", emailClaim);
                _httpContextAccessor.HttpContext.Session.SetString("ruolo", roleClaim);

                emailSession = emailClaim;
            }

            var listMsgUser = _msgProvider.GetAllMessage(emailSession);
            ListaMessaggiModel model = new ListaMessaggiModel();
            model.ListMessage = listMsgUser;
            model.Email = emailSession;
            return View(model);
        }

        public IActionResult Logout ()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            return SignOut("Cookies", "oidc");
        }

        
    }
}
