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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMsgUserHelper _msgUserHelper;

        public HomeController(
            IHttpContextAccessor httpContextAccessor,
            IMsgUserHelper msgUserHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _msgUserHelper = msgUserHelper;
        }

        public IActionResult Index()
        {
            var emailSession = _httpContextAccessor.HttpContext.Session.GetString("email");

            if (emailSession == null)
            {
                try
                {
                    var emailClaim = User.Claims.Where(x => x.Type == "email").Select(k => k.Value).First();
                    var nameClaim = User.Claims.Where(x => x.Type == "nome").Select(k => k.Value).First();
                    var roleClaim = User.Claims.Where(x => x.Type == "ruolo").Select(k => k.Value).First();
                    roleClaim = (roleClaim == "1") ? "Guest" : "Admin";
                    var imgClaim = User.Claims.Where(x => x.Type == "immagine").Select(k => k.Value).First();
                    var infoClaim = User.Claims.Where(x => x.Type == "info").Select(k => k.Value).First();

                    _httpContextAccessor.HttpContext.Session.SetString("nome", nameClaim);
                    _httpContextAccessor.HttpContext.Session.SetString("email", emailClaim);
                    _httpContextAccessor.HttpContext.Session.SetString("ruolo", roleClaim);
                    _httpContextAccessor.HttpContext.Session.SetString("immagine", imgClaim);
                    _httpContextAccessor.HttpContext.Session.SetString("info", infoClaim);
                    emailSession = emailClaim;
                } catch (Exception e)
                {
                    return SignOut("Cookies", "oidc");
                }
            }
            
            var listMsgUser = _msgUserHelper.GetMessaggi(emailSession);
            ListaMessaggiModel model = new ListaMessaggiModel
            {
                ListMessage = listMsgUser,
                Email = emailSession
            };
            return View(model);
        }

        public IActionResult Logout()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            return SignOut("Cookies", "oidc");
        }

    }
}
