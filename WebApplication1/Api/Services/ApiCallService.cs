using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.DataModels;
using WebApplication1.Models.HttpResponse;

namespace WebApplication1.Api.Services
{
    public interface IApiCallService
    {
     
        public Task<HttpResponseModel> GetAllUser();
        public Task<HttpResponseModel> GetAllMessage();
        public Task<HttpResponseModel> GetSingleMessage(string id);
        public Task<HttpResponseModel> DeleteMessage(string id);
        public Task<HttpResponseModel> GetSingleUser(string email);
        public Task<HttpResponseModel> PutMessage(string id, string mes);
        public Task<HttpResponseModel> PutUtente(string id, string mes);
        public  Task<HttpResponseModel> PostUtente(Utente us);
        public Task<HttpResponseModel> PostMessaggio(Messaggio msg);
    }
    public class ApiCallService : IApiCallService
    {
        private readonly IHttpClientFactory _clientFactory;

        private string Token;
        private DateTime Time;
        public ApiCallService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
     
        }


        private async Task<HttpResponseModel> CallHttpRequest(string name, HttpMethod method,string param, string body = null)
        {
            var url = "https://localhost:44330/api/" + name+param;
            var request = new HttpRequestMessage(method,
                 url);
            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("User-Agent", "HttpClientFactory-Sample");
            if (name != "token") request.Headers.Add("Authorization", "Bearer " + Token);
            if(body!= null) request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);            
            
            var responseStream = await response.Content.ReadAsStringAsync();
            
            HttpResponseModel md = new HttpResponseModel { Response = responseStream, Code = response.StatusCode, Operation = name };
            return md;
        }
        private async Task SetToken()
        {
            DateTime time = DateTime.Now;
            TimeSpan diff = time.Subtract(Time);
            if(diff.TotalHours >= 24)
            {
                HttpResponseModel md = await GetToken();
                Time = time;
                Token = md.Response;
            }
        }
        private async Task<HttpResponseModel> GetToken()
        {
            var md = await CallHttpRequest("token", HttpMethod.Get,"");
            return md;
        }

        public async Task<HttpResponseModel> GetAllUser()
        {
            await SetToken();
            var md = await CallHttpRequest("Utente", HttpMethod.Get,"");
            return md;
        }
        public async Task<HttpResponseModel> GetSingleUser(string email)
        {
            await SetToken();
            var md = await CallHttpRequest("Utente/"+email, HttpMethod.Get, "");
            return md;
        }
        public async Task<HttpResponseModel> GetAllMessage()
        {
            await SetToken();
            var md = await CallHttpRequest("Messaggio", HttpMethod.Get,"");
            return md;
        }

        public async Task<HttpResponseModel> GetSingleMessage(string id)
        {
            await SetToken();
            var md = await CallHttpRequest("Messaggio/"+id, HttpMethod.Get, "");
            return md;
        }
        public async Task<HttpResponseModel> DeleteMessage(string id)
        {
            await SetToken();
            var md = await CallHttpRequest("Messaggio/" + id, HttpMethod.Delete, "");
            var md2 = await CallHttpRequest("Messaggio", HttpMethod.Get, "");
            return md2;
        }

        public async Task<HttpResponseModel> PutMessage(string id,string mes)
        {
            await SetToken();
            var md = await CallHttpRequest("Messaggio/" + id, HttpMethod.Put, "?testo=" + mes);
            
            return md;
        }

        public async Task<HttpResponseModel> PutUtente(string id, string mes)
        {
            await SetToken();
            Utente newU = new Utente { Email = id, Nome = mes, Password = "", Ruolo = 1 };
            string body =Newtonsoft.Json.JsonConvert.SerializeObject(newU);
            var md = await CallHttpRequest("Utente/" + id, HttpMethod.Put, "",body);

            return md;
        }

        public async Task<HttpResponseModel> PostUtente(Utente us)
        {
            await SetToken();
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(us);
            var md = await CallHttpRequest("Utente",HttpMethod.Post,"",body);
            return md;
        }

        public async Task<HttpResponseModel> PostMessaggio(Messaggio msg)
        {
            await SetToken();
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
            var md = await CallHttpRequest("Messaggio", HttpMethod.Post, "", body);
            return md;
        }
    }
}
