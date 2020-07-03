﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.DataModels;

namespace WebApplication.DataAccess.SQL.Providers
{
    public interface IMsgProvider
    {
        public bool AddMessage(string testo , string email);
        public List<Messaggio> GetMsg();
        public List<MsgUser> GetAllMessage(string name);
        public void AddLike(string id, string email);
        public void RemoveLike(string id,string email);
    }
    public class MsgProvider : IMsgProvider
    {

        private readonly ApplicationDbContext _DbContext;

        public MsgProvider(ApplicationDbContext DbContext)
        {
            _DbContext = DbContext;
        }
        public bool AddMessage(string testo, string email)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Messaggio msg = new Messaggio { IDMessaggio = Guid.NewGuid().ToString(), Testo = testo, Data = time, Email = email };
            _DbContext.Add(msg);
            var nUser = _DbContext.Utente.Select(o => o.Email).ToList();
            foreach(var z in nUser)
            {
                _DbContext.UtenteLikeMessaggio.Add(new UtenteLikeMessaggio
                {
                    Email = z,
                    IDMessaggio = msg.IDMessaggio,
                    SetLike = 0

                }); ;
            }
            _DbContext.SaveChanges();
            return true;
        }

        public List<Messaggio> GetMsg()
        {
            var b = _DbContext.Messaggio.ToList();
            return b;
        }

        public List<MsgUser> GetAllMessage(string ut = null)
        {

            var msgList = _DbContext.UtenteLikeMessaggio.Where(ulm=> ulm.Email==ut).Join(_DbContext.Messaggio,
                      ulm => ulm.IDMessaggio , m => m.IDMessaggio,
                      (ulm,m) => new {
                          IDMessaggio = ulm.IDMessaggio,
                          SetLike = ulm.SetLike,
                          Testo = m.Testo,
                          Email = m.Email,
                          Data = m.Data,
                          Like = m.NLike
                      }).Join(_DbContext.Utente,
                      msg => msg.Email , u=> u.Email,
                      (msg,u) => new MsgUser {
                          IDMessaggio = msg.IDMessaggio,
                          SetLike = msg.SetLike,
                          Testo = msg.Testo,
                          Nome = u.Nome,
                          Img = u.Img,
                          Data = msg.Data,
                          Like = msg.Like

                      }).OrderByDescending(s => s.Data).ToList();

            var comments = _DbContext.Commento.Where(x => x.IDComRef == null).Join(_DbContext.Utente,
                com => com.Email , u => u.Email,
                (com,u)=> new CommentoModel { 
                    Email = com.Email,
                    Nome = u.Nome,
                    IDMessaggio = com.IDMessaggio,
                    Img = u.Img,
                    TestoCommento = com.TestoCommento,
                    Data = com.Data 
                 }).OrderBy(x => x.Data);


            foreach (var msg in msgList)
            {
               var com=  comments.Where(x => x.IDMessaggio == msg.IDMessaggio).ToList();
                msg.Commenti = com;
            }       
            return msgList;           
          
        }

        public void AddLike(string id, string email)
        {
            var msg = _DbContext.Messaggio.FirstOrDefault(msg => msg.IDMessaggio == id);
            var ulm = _DbContext.UtenteLikeMessaggio.Where(ulm => ulm.IDMessaggio == id).FirstOrDefault(uml => uml.Email == email);
            if (msg != null)
            {
                using (var dbContextTransaction = _DbContext.Database.BeginTransaction())
                {
                    msg.NLike++;
                    ulm.SetLike = 1;
                    _DbContext.SaveChanges();
                    dbContextTransaction.Commit();
                }
            }
        }


        public void RemoveLike(string id,string email)
        {
            var msg = _DbContext.Messaggio.FirstOrDefault(msg => msg.IDMessaggio == id);
            var ulm = _DbContext.UtenteLikeMessaggio.Where(ulm => ulm.IDMessaggio == id).FirstOrDefault(uml => uml.Email == email);
            if (msg != null)
            {
                using (var dbContextTransaction = _DbContext.Database.BeginTransaction())
                {
                    msg.NLike--;
                    ulm.SetLike = 0;
                    _DbContext.SaveChanges();
                    dbContextTransaction.Commit();
                }
            }
        }
    }
}
