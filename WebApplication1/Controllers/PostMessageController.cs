﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DataAccess.SQL.DataModels;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PostMessageController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMsgProvider _msgProvider;

        public PostMessageController (IMsgProvider msgProvider, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _msgProvider = msgProvider;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostMessage(PostMessageViewModel model)
        {
      
            _msgProvider.AddMessage(model.Message, _httpContextAccessor.HttpContext.Session.GetString("email"));
            
            return RedirectToAction("Index", "Home");
        }
    }
}
