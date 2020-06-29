using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.DataAccess.SQL;
using WebApplication.DataAccess.SQL.DataModels;

namespace WebApplication.IdentityServer.Provider
{

    public class UserStore : IUserStore<Utente>, IUserPasswordStore<Utente> , IUserClaimStore<Utente>
    {

        private readonly ApplicationDbContext _DbContext;

        public UserStore(ApplicationDbContext DbContext)
        {
            _DbContext = DbContext;
        }

        public Task AddLoginAsync(Utente user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> CreateAsync(Utente user, CancellationToken cancellationToken)
        {
            _DbContext.Add(user);
            await _DbContext.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(Utente user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _DbContext?.Dispose();
            }
        }

        public Task<Utente> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Utente> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Utente> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await _DbContext.Utente.FirstOrDefaultAsync(op => op.Email == normalizedUserName.ToLower());
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(Utente user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(Utente user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(Utente user, CancellationToken cancellationToken)
        {
           
          /*  byte[] bytes = Encoding.ASCII.GetBytes(user.Password);
            string a = Convert.ToBase64String(bytes);
            var z =Convert.FromBase64String(a);*/
            
            return Task.FromResult(user.Password);
        }

        public Task<string> GetUserIdAsync(Utente user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<string> GetUserNameAsync(Utente user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> HasPasswordAsync(Utente user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(Utente user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(Utente user, string normalizedName, CancellationToken cancellationToken)
        {
            return Task.FromResult((object)null);
        }

        public Task SetPasswordHashAsync(Utente user, string passwordHash, CancellationToken cancellationToken)
        {
            user.Password = passwordHash;

            return Task.FromResult((object)null);
        }

        public Task SetUserNameAsync(Utente user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(Utente user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsAsync(Utente user, CancellationToken cancellationToken)
        {
            List<Claim> claimList =  new List<Claim>();
            claimList.Add(new Claim("email", user.Email));
            claimList.Add(new Claim("pass", user.Password));
            claimList.Add(new Claim("ruolo", user.Ruolo.ToString()));

            IList<Claim> IClaimList = claimList;
            return Task.FromResult(IClaimList);
        }

        public Task AddClaimsAsync(Utente user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(Utente user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(Utente user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Utente>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
