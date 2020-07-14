using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.DataModels;
using WebApplication1.Helper;
using WebApplication1.Models.DataModel;

namespace WebApplication1.Mappers
{
    public interface IMsgUserMapper
    {
        public List<MsgUser> Map(List<Utente> user,
            List<Messaggio> msg,
            List<Commento> commenti,
            List<UtenteLikeMessaggio> ulm, string email);
        public List<MsgUser> MapSingle(List<Utente> user,
            List<Messaggio> msg, 
            List<Commento> commenti, 
            List<UtenteLikeMessaggio> ulm, string email, string emailPanel);

    }
    public class MsgUserMapper : IMsgUserMapper
    {
        private readonly ITimeHelper _timeHelper;

        public MsgUserMapper(ITimeHelper timeHelper)
        {
            _timeHelper = timeHelper;
        }
        public List<MsgUser> Map(List<Utente> user, List<Messaggio> msg, List<Commento> commenti, List<UtenteLikeMessaggio> ulm, string email)
        {
            var subcomment = commenti.Where(x => x.IDComRef != null).Join(user,
                       com => com.Email, u => u.Email,
                          (com, u) => new CommentoModel
                          {
                              Email = com.Email,
                              Nome = u.Nome,
                              IDMessaggio = com.IDComRef,
                              IDCommento = com.IDCommento,
                              Img = u.Img,
                              TestoCommento = com.TestoCommento,
                              Data = _timeHelper.Converter(com.Data)
                          });

            var comments = commenti.Where(x => x.IDComRef == null).Join(user,
                          com => com.Email, u => u.Email,
                          (com, u) => new CommentoModel
                          {
                              Email = com.Email,
                              Nome = u.Nome,
                              IDMessaggio = com.IDMessaggio,
                              IDCommento = com.IDCommento,
                              Img = u.Img,
                              TestoCommento = com.TestoCommento,
                              Data = _timeHelper.Converter(com.Data),
                              SubCommenti = subcomment.Where(x => x.IDMessaggio == com.IDCommento).ToList()                              
                              
                          });

            var msgList = ulm.Where(ulm => ulm.Email == email).Join(msg,
                      ulm => ulm.IDMessaggio, m => m.IDMessaggio,
                      (ulm, m) => new
                      {
                          IDMessaggio = ulm.IDMessaggio,
                          SetLike = ulm.SetLike,
                          Testo = m.Testo,
                          Email = m.Email,
                          Data = m.Data,
                          Like = m.NLike
                      }).OrderByDescending(o => o.Data).Join(user,
                      msg => msg.Email, u => u.Email,
                      (msg, u) => new MsgUser
                      {
                          Email = u.Email,
                          IDMessaggio = msg.IDMessaggio,
                          SetLike = msg.SetLike,
                          Testo = msg.Testo,
                          Nome = u.Nome,
                          Img = u.Img,
                          Data = _timeHelper.Converter(msg.Data),
                          Like = msg.Like,
                          Commenti = comments.Where(x => x.IDMessaggio == msg.IDMessaggio).ToList()

                      }).ToList();

            return msgList;
        }

        public List<MsgUser> MapSingle(List<Utente> user, List<Messaggio> msg, List<Commento> commenti, List<UtenteLikeMessaggio> ulm, string email,string emailPanel)
        {
            var subcomment = commenti.Where(x => x.IDComRef != null).Join(user,
                       com => com.Email, u => u.Email,
                          (com, u) => new CommentoModel
                          {
                              Email = com.Email,
                              Nome = u.Nome,
                              IDMessaggio = com.IDComRef,
                              IDCommento = com.IDCommento,
                              Img = u.Img,
                              TestoCommento = com.TestoCommento,
                              Data = _timeHelper.Converter(com.Data)
                          });

            var comments = commenti.Where(x => x.IDComRef == null).Join(user,
                          com => com.Email, u => u.Email,
                          (com, u) => new CommentoModel
                          {
                              Email = com.Email,
                              Nome = u.Nome,
                              IDMessaggio = com.IDMessaggio,
                              IDCommento = com.IDCommento,
                              Img = u.Img,
                              TestoCommento = com.TestoCommento,
                              Data = _timeHelper.Converter(com.Data),
                              SubCommenti = subcomment.Where(x => x.IDMessaggio == com.IDCommento).ToList()

                          });

            var msgList = ulm.Where(ulm => ulm.Email == email).Join(msg,
                      ulm => ulm.IDMessaggio, m => m.IDMessaggio,
                      (ulm, m) => new
                      {
                          IDMessaggio = ulm.IDMessaggio,
                          SetLike = ulm.SetLike,
                          Testo = m.Testo,
                          Email = m.Email,
                          Data = m.Data,
                          Like = m.NLike
                      }).OrderByDescending(o => o.Data).Where(k=> k.Email == emailPanel).Join(user,
                      msg => msg.Email, u => u.Email,
                      (msg, u) => new MsgUser
                      {
                          Email = u.Email,
                          IDMessaggio = msg.IDMessaggio,
                          SetLike = msg.SetLike,
                          Testo = msg.Testo,
                          Nome = u.Nome,
                          Img = u.Img,
                          Data = _timeHelper.Converter(msg.Data),
                          Like = msg.Like,
                          Commenti = comments.Where(x => x.IDMessaggio == msg.IDMessaggio).ToList()

                      }).ToList();

            return msgList;
        }

    }
}
