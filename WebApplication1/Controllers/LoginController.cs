using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DataAccess.SQL.DataModels;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserProvider _userProvider;

        public LoginController(IUserProvider userProvider, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userProvider = userProvider;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            //_userProvider.GetMsg();
            Utente user = _userProvider.GetUserMail(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("CustomError", "Email non esistente");
                return View("Index");
            }

            if (!(user.Password == model.Password)) return View("Index");
            string role = (user.Ruolo == 1) ? "Viewer" : "Admin";
            _httpContextAccessor.HttpContext.Session.SetString("nome", user.Nome);
            _httpContextAccessor.HttpContext.Session.SetString("email", model.Email);
            _httpContextAccessor.HttpContext.Session.SetString("ruolo", role);

            return RedirectToAction("Index", "Home");
        }

  
    }
}
