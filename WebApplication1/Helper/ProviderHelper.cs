using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication1.Mappers;
using WebApplication1.Models.DataModel;

namespace WebApplication1.Helper
{
    public interface IProviderHelper
    {
        public List<MsgUser> GetMessaggi(string email);
    }
    public class ProviderHelper : IProviderHelper
    {
        private readonly IUserProvider _userProvider;
        private readonly IMsgProvider _msgProvider;
        private readonly ICommentiProvider _commentiProvider;
        private readonly IUtenteLikeMessaggioProvider _ulmProvider;
        private readonly IMapper _mapper;

        public ProviderHelper (
            IUserProvider userProvider, 
            IMsgProvider msgProvider , 
            ICommentiProvider commentiProvider,
            IUtenteLikeMessaggioProvider ulmProvider,
            IMapper mapper)
        {
            _userProvider = userProvider;
            _msgProvider = msgProvider;
            _commentiProvider = commentiProvider;
            _ulmProvider = ulmProvider;
            _mapper = mapper;
        }


        public List<MsgUser> GetMessaggi(string email) 
        {
            var msg = _msgProvider.GetMsg();
            var user = _userProvider.GetAllUser();
            var ulm = _ulmProvider.GetAllUlm();
            var com = _commentiProvider.GetAllCommento();

           return _mapper.Map(user,msg,com,ulm,email);

          
        }
    }
}
