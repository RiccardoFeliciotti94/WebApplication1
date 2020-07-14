using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.DataModels;
using WebApplication1.Helper;
using WebApplication1.Models.DataModel;

namespace WebApplication1.Mappers
{
    public interface ICommentoModelMapper
    {
        public CommentoModel Map(List<Utente> user,
            Commento commenti);
    }
    public class CommentoModelMapper : ICommentoModelMapper
    {
        private readonly ITimeHelper _timeHelper;

        public CommentoModelMapper (ITimeHelper timeHelper)
        {
            _timeHelper = timeHelper;
        }
        public CommentoModel Map(List<Utente> user, Commento commenti)
        {
            var u = user.FirstOrDefault(x => x.Email == commenti.Email);
            return new CommentoModel
            {
                Email = commenti.Email,
                Nome = u.Nome,
                IDMessaggio = commenti.IDComRef,
                IDCommento = commenti.IDCommento,
                Img = u.Img,
                TestoCommento = commenti.TestoCommento,
                Data = _timeHelper.Converter(commenti.Data),
                SubCommenti = null
            };            

        }
    }
}
