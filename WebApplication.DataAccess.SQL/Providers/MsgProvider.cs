using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
        public bool AddMessage(string testo, string email);
        public List<Messaggio> GetMsg();
        public void AddLike(string id, string email);
        public void RemoveLike(string id, string email);
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
            string time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            Messaggio msg = new Messaggio { IDMessaggio = Guid.NewGuid().ToString(), Testo = testo, Data = time, Email = email };
            _DbContext.Add(msg);
            var nUser = _DbContext.Utente.Select(o => o.Email).ToList();
            foreach (var z in nUser)
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


        public void RemoveLike(string id, string email)
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
