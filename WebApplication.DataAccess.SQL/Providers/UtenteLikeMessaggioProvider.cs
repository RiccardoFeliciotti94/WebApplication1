using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApplication.DataAccess.SQL.DataModels;

namespace WebApplication.DataAccess.SQL.Providers
{
    public interface IUtenteLikeMessaggioProvider
    {
        public List<UtenteLikeMessaggio> GetAllUlm();

    }
    public class UtenteLikeMessaggioProvider : IUtenteLikeMessaggioProvider
    {
        private readonly ApplicationDbContext _DbContext;

        public UtenteLikeMessaggioProvider(ApplicationDbContext DbContext)
        {
            _DbContext = DbContext;
        }
        public List<UtenteLikeMessaggio> GetAllUlm()
        {
            return _DbContext.UtenteLikeMessaggio.ToList();
        }
    }
}
