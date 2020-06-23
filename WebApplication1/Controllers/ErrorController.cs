using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models.Error;

namespace WebApplication1.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
             Exception e =exceptionFeature.Error;
            ErrorModel er = new ErrorModel { Excep = e.Message };
            return View(er);
            }

            return View(new ErrorModel()); 
        }
        [Route("401")]
        public IActionResult AppErr()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                Exception e = exceptionFeature.Error;
                ErrorModel er = new ErrorModel { Excep = e.Message };
                return View(er);
            }

            return View(new ErrorModel { Excep="401"});
            
        }
        [Route("404")]
        public IActionResult AppErr2()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                Exception e = exceptionFeature.Error;
                ErrorModel er = new ErrorModel { Excep = e.Message };
                return View(er);
            }

            return View(new ErrorModel { Excep = "404" });

        }
    }
}
