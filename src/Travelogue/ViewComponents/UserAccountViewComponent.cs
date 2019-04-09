using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Travelogue.Models;

namespace Travelogue.ViewComponents
{
    public class UserAccountViewComponent : ViewComponent
    {
        private UserManager<TravelUser> _userManager;

        public UserAccountViewComponent(UserManager<TravelUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync((ClaimsPrincipal)User);
    
            return View(user);
        }
    }
}
