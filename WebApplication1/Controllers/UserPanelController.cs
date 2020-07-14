using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DataAccess.SQL.DataModels;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication1.Helper;
using WebApplication1.Models.UtentePanel;

namespace WebApplication1.Controllers
{
    public class UserPanelController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMsgUserHelper _msgUserHelper;
        private readonly IUserProvider _userProvider;

        public UserPanelController(
            IHttpContextAccessor httpContextAccessor,
            IMsgUserHelper msgUserHelper,
            IUserProvider userProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _msgUserHelper = msgUserHelper;
            _userProvider = userProvider;
        }

        [Route("userpanel/{id?}")]
        public IActionResult Index(string? id)
        {
            string email = _httpContextAccessor.HttpContext.Session.GetString("email");
            if (id == null) return Redirect(email);
            if (email == id)
            {
                UtentePanelModel upm = new UtentePanelModel
                {
                    Email = email,
                    Host = email,
                    Img = _httpContextAccessor.HttpContext.Session.GetString("immagine"),
                    Nome = _httpContextAccessor.HttpContext.Session.GetString("nome"),
                    Messaggi = _msgUserHelper.GetMessaggiOneUser(email, email),
                    Info = _httpContextAccessor.HttpContext.Session.GetString("info")
                };

                return View(upm);
            }
            Utente ut = _userProvider.GetUserMail(id);
            if (ut != null)
            {
                UtentePanelModel upm = new UtentePanelModel
                {
                    Email = id,
                    Host = email,
                    Img = ut.Img,
                    Nome = ut.Nome,
                    Info = ut.Info,
                    Messaggi = _msgUserHelper.GetMessaggiOneUser(email, id)
                };
                return View(upm);
            }



            return Redirect(email);
        }
    }
}
