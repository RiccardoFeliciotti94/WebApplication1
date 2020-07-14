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

        public bool EditName(string Email,string Name);
        public bool EditInfo(string Email, string Info);

        public bool Update(string Email, Utente user);

        public bool EditPassword(string Email, string Name);
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

        public bool EditName(string Email,string Name)
        {
            if (Name == null || Name == "") return false;
            var user = _DbContext.Utente.FirstOrDefault(u => u.Email == Email);
            if (user == null) return false;
            user.Nome = Name;
            try
            {
                _DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool EditPassword(string Email, string Pass)
        {
        
            var user = _DbContext.Utente.FirstOrDefault(u => u.Email == Email);
            if (user == null) return false;
            user.Password = Pass;
            try
            {
                _DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool EditInfo(string Email, string Info)
        {
            if (Info == null ) return false;
            var user = _DbContext.Utente.FirstOrDefault(u => u.Email == Email);
            if (user == null) return false;
            user.Info = Info;
            try
            {
                _DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public bool Update(string Email, Utente user)
        {
            if (user == null) return false;
            var oldUser = _DbContext.Utente.FirstOrDefault(u => u.Email == Email);
            if (oldUser == null) return false;
            oldUser = user;
            try
            {
                _DbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;

        }
    }
}
