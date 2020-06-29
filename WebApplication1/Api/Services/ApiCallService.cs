using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json.Linq;
using System;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string Token;

        public ApiCallService(IHttpClientFactory clientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _clientFactory = clientFactory;
            _httpContextAccessor = httpContextAccessor;

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
            await CallHttpRequest("Messaggio/" + id, HttpMethod.Delete, "");
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

        private async Task<bool> SetToken()
        {
         
            var client = new HttpClient();            
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return false;
            }
            // request token
            /* var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
             {
                 Address = disco.TokenEndpoint,
                 ClientId = "client",
                 ClientSecret = "secret",
                 Scope = "api1.get"
             });*/
            
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "ro.client",
                ClientSecret = "secret",
                Scope = "api1.get",

                UserName = "carlo@cr.it",
                Password = "Abc123456789!"
            });
            
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return false;
            }
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");
            Token = tokenResponse.AccessToken;
            return true;
            // call api
           /* var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            

            var response = await apiClient.GetAsync("https://localhost:44330/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }*/
        }
    }
}
