using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL.Providers;
using WebApplication1.Mappers;
using WebApplication1.Models.DataModel;

namespace WebApplication1.Helper
{
    public interface IMsgUserHelper
    {
        public List<MsgUser> GetMessaggi(string email);
        public List<MsgUser> GetMessaggiOneUser(string email, string emailPanel);
    }
    public class MsgUserHelper : IMsgUserHelper
    {
        private readonly IUserProvider _userProvider;
        private readonly IMsgProvider _msgProvider;
        private readonly ICommentiProvider _commentiProvider;
        private readonly IUtenteLikeMessaggioProvider _ulmProvider;
        private readonly IMsgUserMapper _mapper;

        public MsgUserHelper (
            IUserProvider userProvider, 
            IMsgProvider msgProvider , 
            ICommentiProvider commentiProvider,
            IUtenteLikeMessaggioProvider ulmProvider,
            IMsgUserMapper mapper)
        {
            _userProvider = userProvider;
            _msgProvider = msgProvider;
            _commentiProvider = commentiProvider;
            _ulmProvider = ulmProvider;
            _mapper = mapper;
        }


        public List<MsgUser> GetMessaggi(string email) 
        {            
            return _mapper.Map(
                _userProvider.GetAllUser(),
                _msgProvider.GetMsg(),
                _commentiProvider.GetAllCommento(),
                _ulmProvider.GetAllUlm(),
                email);      
        }

        public List<MsgUser> GetMessaggiOneUser(string email,string emailPanel)
        {
            return _mapper.MapSingle(
                _userProvider.GetAllUser(),
                _msgProvider.GetMsg(),
                _commentiProvider.GetAllCommento(),
                _ulmProvider.GetAllUlm(),
                email,emailPanel);
        }

    }
}
