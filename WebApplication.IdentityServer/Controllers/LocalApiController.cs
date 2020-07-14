using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DataAccess.SQL.DataModels;
using static IdentityServer4.IdentityServerConstants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication.IdentityServer.Controllers
{
    [Route("localapi")]
    [Authorize(LocalApi.PolicyName)]
    public class LocalApiController : ControllerBase
    {
        private readonly UserManager<Utente> _userManager;

        public LocalApiController(UserManager<Utente> userManager)
        {
            _userManager = userManager;
        }
        // GET: api/<LocalApiController>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(string id,string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            string tk = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result =await _userManager.ResetPasswordAsync(user, tk, newPassword);
            if (result.Succeeded)
            {
                return Ok();
            }
            else return BadRequest();
            

        }
    }
}
