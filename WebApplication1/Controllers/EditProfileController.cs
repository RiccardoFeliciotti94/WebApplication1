using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplication.DataAccess.SQL.DataModels;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication1.Models.ProfileInfo;

namespace WebApplication1.Controllers
{
    public class EditProfileController : Controller
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserProvider _userProvider;
        private readonly IConfiguration _Configuration;
        private readonly IHttpClientFactory _clientFactory;

        public EditProfileController(IHttpContextAccessor httpContextAccessor,
            IUserProvider userProvider,
            IConfiguration configuration,
            IHttpClientFactory clientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _userProvider = userProvider;
            _Configuration = configuration;
            _clientFactory = clientFactory;

        }
        public IActionResult Index()
        {
            ProfileInfoModel model = new ProfileInfoModel
            {
                Info = _httpContextAccessor.HttpContext.Session.GetString("info"),
                Name = _httpContextAccessor.HttpContext.Session.GetString("nome"),
                Img = _httpContextAccessor.HttpContext.Session.GetString("immagine"),
            };
            return View(model);
        }

        public IActionResult EditName(ProfileInfoModel model)
        {
            if (_userProvider.EditName(_httpContextAccessor.HttpContext.Session.GetString("email"), model.Name.Trim()))
            {
                _httpContextAccessor.HttpContext.Session.SetString("nome", model.Name);
                return RedirectToAction("Index");
            }
            else return View(model);
        }

        public IActionResult EditInfo(ProfileInfoModel model)
        {
            if (_userProvider.EditInfo(_httpContextAccessor.HttpContext.Session.GetString("email"), model.Info.Trim()))
            {
                _httpContextAccessor.HttpContext.Session.SetString("info", model.Info);
                return RedirectToAction("Index");
            }
            else return View(model);
        }

        public IActionResult EditImg(ProfileInfoModel model)
        {
            if (model.Img == null) return View();
            var user = _userProvider.GetUserMail(_httpContextAccessor.HttpContext.Session.GetString("email"));
            user.Img = model.Img;
            if (_userProvider.Update(_httpContextAccessor.HttpContext.Session.GetString("email"), user))
            {
             
                _httpContextAccessor.HttpContext.Session.SetString("immagine", model.Img);
                return RedirectToAction("Index");
            }
            return View();


        }


        public async Task<IActionResult> EditPassword(ProfileInfoModel model)
        {
            if (model.NewPassword != model.NewPasswordAgain) return View();

            var email = _httpContextAccessor.HttpContext.Session.GetString("email");

            var url = "https://localhost:5001/localapi/" + email + "?newPassword=" + model.NewPassword;
            var request = new HttpRequestMessage(HttpMethod.Put,
                 url);
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            request.Headers.Add("Authorization", "Bearer " + token);

            var client2 = _clientFactory.CreateClient();
            var response = await client2.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else return View();
        }
    }
}
