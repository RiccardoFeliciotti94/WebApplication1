using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication.DataAccess.SQL.DataModels;
using WebApplication1.Api.Services;
using WebApplication1.Models.HttpResponse;

namespace WebApplication1.Controllers
{
    [Authorize(Policy="Admin")]
    public class AdminPanelController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IApiCallService _httpResponse;
      

        public AdminPanelController( IApiCallService httpResponse , IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            _httpResponse = httpResponse;
    
        }
        public IActionResult Index()
        {           
            HttpResponseModel md = new HttpResponseModel { Response = "", Operation = ""};
            return View(md);     
        }


        public async Task<IActionResult> GetUser(HttpResponseModel model)
        {
            
            if(model.Email==null) {  var md= await _httpResponse.GetAllUser();
            List<Utente> a = JsonConvert.DeserializeObject<List<Utente>>(md.Response);
            
            md.ListUser = a;
                ModelState.Clear();
                return View("Index", md);}
            else {
                var md = await _httpResponse.GetSingleUser(model.Email);
                if(md.Code.ToString()!="OK")
                {
                    ModelState.AddModelError("CustomError", "Email non esistente");
                    return View("Index",md);
                }
                Utente a = JsonConvert.DeserializeObject<Utente>(md.Response);
                List<Utente> f = new List<Utente>() { a };                
                md.ListUser = f;
                ModelState.Clear();
                return View("Index", md);
            }
            
        }

        public async Task<IActionResult> GetMessage(HttpResponseModel model)
        {
            if(model.IDMes==null)
            {
                var md = await _httpResponse.GetAllMessage();
                List<Messaggio> msglist = JsonConvert.DeserializeObject<List<Messaggio>>(md.Response);
                md.ListMessage = msglist;
                ModelState.Clear();
                return View("Index", md);
            } else
            {
                var md = await _httpResponse.GetSingleMessage(model.IDMes);
                if ((int)md.Code != 200)
                {
                    ModelState.AddModelError("CustomErrorIDMes", "ID messaggio non esistente");
                    return View("Index", md);
                }
                Messaggio msglist = JsonConvert.DeserializeObject<Messaggio>(md.Response);
                List<Messaggio> f = new List<Messaggio>{ msglist};
                md.ListMessage = f;
                ModelState.Clear();
                return View("Index", md);
            }
            
        }

        public async Task<IActionResult> DeleteMessage(string id) {

            var md = await _httpResponse.DeleteMessage(id);
            List<Messaggio> msglist = JsonConvert.DeserializeObject<List<Messaggio>>(md.Response);
            md.ListMessage = msglist;
            ModelState.Clear();
            return View("Index", md);
        }

        public async Task<IActionResult> PutMessage (HttpResponseModel model , string id = null)
        {
            var md = await _httpResponse.PutMessage(id ?? model.IDMes, model.NewMessage);
            md.IDMes = null;
            md.NewMessage = model.NewMessage;
            if(id==null)
            {
                ModelState.Clear();
                return View("Index", md);
            }else
            {
                var md2 = await _httpResponse.GetAllMessage();
                List<Messaggio> msglist = JsonConvert.DeserializeObject<List<Messaggio>>(md2.Response);
                md2.ListMessage = msglist;
                ModelState.Clear();
                return View("Index", md2);
            }
            
        }

        public async Task<IActionResult> PutUtente (HttpResponseModel model)
        {
            var md = await _httpResponse.PutUtente(model.Email, model.NewName);
            ModelState.Clear();
            md.NewName = model.NewName;           
            return View("Index", md);
        }

       public async Task<IActionResult> PostUtente (HttpResponseModel model)
        {
            Utente user = new Utente {Email=model.Email,Nome=model.NewName,Password=model.Pass,Ruolo=model.Ruolo };
            var md = await _httpResponse.PostUtente(user);
            ModelState.Clear();
            return View("Index",md);
        }

        public async Task<IActionResult> PostMessaggio(HttpResponseModel model)
        {
            Messaggio msg = new Messaggio { 
                Email = model.Email, 
                Testo= model.NewMessage,                                          
                Data= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 
                IDMessaggio= Guid.NewGuid().ToString()
            };
            var md = await _httpResponse.PostMessaggio(msg);
            ModelState.Clear();
            return View("Index", md);
        }


    }
}
