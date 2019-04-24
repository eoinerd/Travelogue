using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Travelogue.Extensions
{
    public class CustomClaimsCookieSignInHelper<TIdentityUser> where TIdentityUser : IdentityUser
    {
        private readonly SignInManager<TIdentityUser> _signInManager;
        private readonly UserManager<TIdentityUser> _userManager;

        public CustomClaimsCookieSignInHelper(SignInManager<TIdentityUser> signInManager, UserManager<TIdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task SignInUserAsync(TIdentityUser user, bool isPersistent, IEnumerable<Claim> customClaims)
        {
            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
            if (customClaims != null && claimsPrincipal?.Identity is ClaimsIdentity)
            {
                var claimsIdentity = (ClaimsIdentity) claimsPrincipal?.Identity;
                claimsIdentity.AddClaims(customClaims);
            }

            await _userManager.AddClaimsAsync(user, customClaims);

            await _signInManager.SignInAsync(user, isPersistent);
        }
    }
}
