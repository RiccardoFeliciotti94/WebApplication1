using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication1.Mappers;
using WebApplication1.Models.DataModel;

namespace WebApplication1.Helper
{
    public interface ICommentoModelHelper
    {
        public CommentoModel GetCommentoModelAsync(string idCom);
    }
    public class CommentoModelHelper : ICommentoModelHelper
    {

        private readonly IUserProvider _userProvider;
        private readonly ICommentiProvider _commentiProvider;
        private readonly ICommentoModelMapper _mapper;

        public CommentoModelHelper(
           IUserProvider userProvider,
           ICommentiProvider commentiProvider,
           ICommentoModelMapper mapper)
        {
            _userProvider = userProvider;
            _commentiProvider = commentiProvider;
            _mapper = mapper;
        }

        public CommentoModel GetCommentoModelAsync(string idCom)
        {
            return _mapper.Map(_userProvider.GetAllUser(), _commentiProvider.GetSingleCommnto(idCom));
        }
    }
}
