﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            var msgList=_DbContext.Messaggio.Select(x => x.IDMessaggio).ToList();
            _DbContext.Add(user);            
            foreach( var mes in msgList)
            {
                UtenteLikeMessaggio ulm = new UtenteLikeMessaggio { Email = user.Email, IDMessaggio = mes, SetLike = 0 };
                _DbContext.UtenteLikeMessaggio.Add(ulm);
            }
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

        public async Task<Utente> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _DbContext.Utente.FirstOrDefaultAsync(op => op.Email == userId);
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
            List<Claim> claimList = new List<Claim>
            {
                new Claim("email", user.Email),
                new Claim("pass", user.Password),
                new Claim("ruolo", user.Ruolo.ToString()),
                new Claim("immagine", user.Img)
            };

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
