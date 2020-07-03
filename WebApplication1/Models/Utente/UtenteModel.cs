using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models.Utentes
{
    public class UtenteModel
    {
        private readonly HttpContext _httpContext;

        public UtenteModel(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }
        public string Nome { get { return _httpContext.Session.GetString("nome"); } }
        public string Email { get { return _httpContext.Session.GetString("email"); } }

        public string Ruolo { get { return _httpContext.Session.GetString("ruolo"); } }

        public string Img { get { return _httpContext.Session.GetString("immagine"); } }



    }
}
