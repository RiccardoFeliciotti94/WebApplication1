using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.DataModels;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication.IdentityServer.Models;


namespace WebApplication1.Controllers
{

    public class AccountController : Controller
    {
        private readonly IUserProvider _userProvider;

        private readonly UserManager<Utente> _userManager;
        private readonly SignInManager<Utente> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;


        public AccountController( IUserProvider userProvider,
              UserManager<Utente> userManager,
            SignInManager<Utente> signInManager,
            IIdentityServerInteractionService interaction,
            IEventService events
           )
        {
            _userProvider = userProvider;
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _events = events;
     
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View( new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _userProvider.ConnectWithPassword(model.Email, model.Password);
            // if (!result)
            // {                 
            //     return View(new LoginViewModel { ReturnUrl = model.ReturnUrl });
         //    }  
            // var user = _userProvider.GetUserMail(model.Email);

            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            /* Utente user = new Utente { Email = model.Email, Password = model.Password, Nome= "Franco",Ruolo = 1 };
              var z = await _userManager.CreateAsync(user,user.Password);
             List<Claim> claims = new List<Claim>();
              if(z.Succeeded)
              {
                claims.Add(new Claim("email", user.Email));
                claims.Add(new Claim("ruolo", user.Ruolo.ToString()));

            }
              await _userManager.AddClaimsAsync(user, claims);
               await _signInManager.SignInAsync(user,false);*/

            await _signInManager.PasswordSignInAsync(model.Email,model.Password, false, false);

             //await _events.RaiseAsync(new UserLoginSuccessEvent(us.UserName, user.Email, user.Email, clientId: "mvc"));
            await _events.RaiseAsync(new UserLoginSuccessEvent(model.Email, model.Email, model.Email, clientId: "mvc"));
            var z3 = HttpContext.User.Identity.IsAuthenticated;
            
            return Redirect(model.ReturnUrl);
            
        }
    }
}
