using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.DataModels;

namespace WebApplication.DataAccess.SQL.Providers
{
    public interface IUserProvider
    {
        public List<Utente> GetAllUser();
        public bool AddUser(Utente model);
        public Utente GetUserMail(string Email);
        public Utente GetUtenteWithMailandPassword(string Email, string pass);
        public Task<bool> ConnectWithPassword(string email, string password);
    }
    public class UserProvider : IUserProvider 
    {
        private readonly ApplicationDbContext _DbContext;

        public UserProvider(ApplicationDbContext DbContext) {
            _DbContext = DbContext;
        }

        public List<Utente> GetAllUser()
        {
            var userList=_DbContext.Utente.ToList();
            return userList;
        }

        public bool AddUser(Utente model)
        {           
            _DbContext.Add<Utente>(model);
            try
            {
               _DbContext.SaveChanges();
            } catch(Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
            
            return true;
        }

        public Utente GetUserMail(string Email)
        {
            
            var user=_DbContext.Utente.FirstOrDefault(utente => utente.Email == Email);
            return user;
        }

        public Utente GetUtenteWithMailandPassword(string Email, string pass)
        {
            var user = _DbContext.Utente.FirstOrDefault(utente => utente.Email == Email);
            /* byte[] bytes = Encoding.ASCII.GetBytes(user.Password);
             string a = Convert.ToBase64String(bytes);
             var z = Convert.FromBase64String(user.Password); */
            var z = Convert.FromBase64String(user.Password);
            string passEncoded = Encoding.ASCII.GetString(z);
            if (passEncoded == pass) return user;
            else return null;
        }

        public Task<bool> ConnectWithPassword (string email, string password)
        {
            var user = _DbContext.Utente.FirstOrDefault(utente => utente.Email == email);
            if (user == null) return Task.FromResult(false);
            var z = Convert.FromBase64String(user.Password);
            string passEncoded = Encoding.ASCII.GetString(z);
            if (passEncoded != password) return Task.FromResult(false);
            return Task.FromResult(true);
        }

    }
}
