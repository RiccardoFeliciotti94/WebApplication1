using IdentityServer4;
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
        private readonly UserManager<Utente> _userManager;
        private readonly SignInManager<Utente> _signInManager;
        private readonly IEventService _events;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IdentityServerTools _tools;

        public AccountController(
            UserManager<Utente> userManager,
            SignInManager<Utente> signInManager,
            IEventService events,
            IIdentityServerInteractionService interactionService,
            IdentityServerTools tools)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _events = events;
            _interactionService = interactionService;
            _tools = tools;
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutReq = await  _interactionService.GetLogoutContextAsync(logoutId);
            if(string.IsNullOrEmpty(logoutReq.PostLogoutRedirectUri))
            {
                return RedirectToAction("Index", "Home");
            }
            return Redirect(logoutReq.PostLogoutRedirectUri);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View( new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email,model.Password, false, false);
            
           
            if (result.Succeeded)
            {
                await _events.RaiseAsync(new UserLoginSuccessEvent(model.Email, model.Email, model.Email, clientId: "mvc"));
            
                return Redirect(model.ReturnUrl);
            }
            ModelState.AddModelError("CustomErrorInputNotCorrect", "Password o Email non corretta");
            return View(model);
            
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            return View(new RegisterViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model.Password != model.Password2)
            {
                ModelState.AddModelError("CustomErrorDiffPass", "Password non corrispondono tra loro");
                return View(model);
            }
            Utente user = new Utente { Email = model.Email, Password = model.Password, Nome = model.Username, Ruolo = 1 };
            var z = await _userManager.CreateAsync(user, user.Password);
            
            if( z.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(model.ReturnUrl);
            }
            ModelState.AddModelError("CustomErrorEmailAlready", "Email già usata");
            return View(model);

        }

       
       
    }
}
