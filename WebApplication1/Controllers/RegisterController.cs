using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DataAccess.SQL;
using WebApplication1.Models;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication.DataAccess.SQL.DataModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Controllers
{
    public class RegisterController : Controller
    {
        
        private readonly IUserProvider _userProvider;

        public RegisterController(IUserProvider userProvider)
        {
            _userProvider = userProvider;
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
           
            
            Utente esito = _userProvider.GetUserMail(model.Email);
            if(esito==null)
            {
                Utente NewUser = new Utente { Email = model.Email,Nome=model.Username,Password=model.Password};
                _userProvider.AddUser(NewUser);

            } else return View("Index");


            return RedirectToAction("Index","Login");
           
        }
    }
}
