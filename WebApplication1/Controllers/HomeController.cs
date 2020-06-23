using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication1.Models.ListaMessaggi;

namespace WebApplication1.Controllers
{
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
            var userEmail = _httpContextAccessor.HttpContext.Session.GetString("email");
            var listMsgUser = _msgProvider.GetAllMessage(userEmail);
            ListaMessaggiModel model = new ListaMessaggiModel();
            model.ListMessage = listMsgUser;
            model.Email = userEmail;
            return View(model);
        }

        
    }
}
