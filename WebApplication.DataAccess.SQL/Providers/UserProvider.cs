using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using WebApplication.DataAccess.SQL.DataModels;

namespace WebApplication.DataAccess.SQL.Providers
{
    public interface IUserProvider
    {
        public void GetAllUser();
        public bool AddUser(Utente model);
        public Utente GetUserMail(string Email);
    }
    public class UserProvider : IUserProvider
    {
        private readonly ApplicationDbContext _DbContext;

        public UserProvider(ApplicationDbContext DbContext) {
            _DbContext = DbContext;
        }

        public void GetAllUser()
        {
            var a=_DbContext.Utente.ToList();
        }

        public bool AddUser(Utente model)
        {           
            _DbContext.Add<Utente>(model);
            try
            {
               _DbContext.SaveChanges();
            } catch(Exception e) {
                return false;
            }
            
            return true;
        }

        public Utente GetUserMail(string Email)
        {
            
            var user=_DbContext.Utente.FirstOrDefault(utente => utente.Email == Email);
            return user;
        }

    }
}
