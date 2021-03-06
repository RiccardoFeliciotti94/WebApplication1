﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApplication.DataAccess.SQL.DataModels;

namespace WebApplication.DataAccess.SQL.Providers
{
    public interface ICommentiProvider
    {
        public bool AddCommento(string testo, string email, string idmessaggio, string idRefCom = null);

        public bool RemoveCommento(string id);

        public Commento GetSingleCommnto(string id);

        public List<Commento> GetAllCommento();

    }
    public class CommentiProvider : ICommentiProvider
    {

        private readonly ApplicationDbContext _DbContext;

        public CommentiProvider(ApplicationDbContext DbContext)
        {
            _DbContext = DbContext;
        }
        public bool AddCommento(string testo,string email,string idmessaggio, string idRefCom = null)
        {
            //string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Commento com = new Commento { 
                Email = email, Data = DateTime.Now, 
                IDComRef = idRefCom , TestoCommento = testo ,
                IDMessaggio = idmessaggio , IDCommento = Guid.NewGuid().ToString()
            };
            _DbContext.Add(com);
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

        public List<Commento> GetAllCommento()
        {
            return _DbContext.Commento.OrderByDescending(s => s.Data).ToList();
        }

        public Commento GetSingleCommnto(string id)
        {
            throw new NotImplementedException();
        }

        public bool RemoveCommento(string id)
        {
            throw new NotImplementedException();
        }
    }
}
