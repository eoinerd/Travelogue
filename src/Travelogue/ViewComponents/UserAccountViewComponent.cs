using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfigurationRoot _config;

        public UserAccountViewComponent(UserManager<TravelUser> userManager, IConfigurationRoot config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync((ClaimsPrincipal)User);

            if(user == null)
            {
                return View(new TravelUser());
            }

            user.Image = _config["ImageSettings:RootUrl"] + user.Image;
            return View(user);
        }
    }
}
