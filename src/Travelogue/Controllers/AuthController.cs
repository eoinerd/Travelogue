using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;
using Travelogue.ViewModels;
using Travelogue.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http.Authentication;
using Travelogue.Extensions;

namespace Travelogue.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<TravelUser> _signInManager;
        private CustomClaimsCookieSignInHelper<TravelUser> _customClaimsCookieSignInHelper;
        private UserManager<TravelUser> _userManager;
        private readonly IImageWriter _imageWriter;
        private readonly IConfigurationRoot _config;
        private IHostingEnvironment _hostingEnvironment;

        public AuthController(SignInManager<TravelUser> signInManager, UserManager<TravelUser> userManager, 
            IImageWriter imageWriter, IConfigurationRoot config, IHostingEnvironment hostingEnvironment,
            CustomClaimsCookieSignInHelper<TravelUser> customClaimsCookieSignInHelper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _imageWriter = imageWriter;
            _config = config;
            _hostingEnvironment = hostingEnvironment;
            _customClaimsCookieSignInHelper = customClaimsCookieSignInHelper;
        }

        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Travel");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel vm, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var signInResult =  await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);

                if(signInResult.Succeeded)
                {                
                    var user =  await _userManager.FindByNameAsync(vm.Username);

                    // Add custom claim for ProfileImage
                    var customClaims = new[]
                    {
                        new Claim("ProfileImage", user.Image)
                    };

                    // Use the CustomClaimsCookiesSignInHelper which inherits from IdentityUser
                    await _customClaimsCookieSignInHelper.SignInUserAsync(user, true, customClaims);

                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Index", "Travel");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("Password","Username or password incorrect");
                }
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel vm, IFormFile image, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.FindByEmailAsync(vm.Email) == null)
                {
                    // use ImageWriter to upload image
                    var secureFileName = await _imageWriter.UploadImage(image);
                    var path = Path.Combine(_hostingEnvironment.WebRootPath + "\\img\\", secureFileName);

                    ImageResizeHelper.ResizeImageToSquare(path);

                    var user = new TravelUser()
                    {
                        UserName = vm.Username,
                        Email = vm.Email,
                        PhoneNumber = vm.PhoneNumber.ToString(),
                        Image = "/img/" + secureFileName //_config["ImageSettings:RootImagePath"] + secureFileName
                    };

                    // Create a new User with the userManager
                    var res =  await _userManager.CreateAsync(user, vm.Password);
                    
                    if (res.Errors.Any())
                        return View();

                    // Sign in new user asynchronously
                    var signInResult = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);

                    if (signInResult.Succeeded)
                    {
                        var customClaims = new[]
                        {
                            new Claim("ProfileImage", user.Image)
                        };

                        // Add the custom cooke to claims
                        await _customClaimsCookieSignInHelper.SignInUserAsync(user, true, customClaims);
                    }                 
                }
     
                if (string.IsNullOrWhiteSpace(returnUrl))
                {
                    return RedirectToAction("Trips", "Travel");
                }
                else
                {
                    return Redirect(returnUrl);
                }            
            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ViewProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return View(new TravelUser());
            }

            if (!user.Image.Contains("/img/"))
            {
                user.Image = "/img/" + user.Image;
            }

            return View(user);
        }
        public async Task<ActionResult> Logout()
        {
            if(User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }

            return RedirectToAction("Index", "Travel");
        }

        public async Task<IActionResult> ExternalLoginCallback()
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            //info.Principal //the IPrincipal with the claims from facebook
            //info.ProviderKey //an unique identifier from Facebook for the user that just signed in
            //info.LoginProvider //a string with the external login provider name, in this case Facebook

            //to sign the user in if there's a local account associated to the login provider
             var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);            
            //result.Succeeded will be false if there's no local associated account 

            if (!result.Succeeded)
            {
                var issuerId = info.ProviderKey;

                var fbProfileImageUrl = "http://graph.facebook.com/" + issuerId + "/picture";

                // Get the users email and name from FB API
                var email = info.Principal.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
                var name = info.Principal.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value;

                var user = new TravelUser()
                {
                    UserName = name,
                    Email = "fb" + email,
                    PhoneNumber = string.Empty,
                    Image = fbProfileImageUrl
                };

                var password = Guid.NewGuid().ToString();
                await _userManager.CreateAsync(user, password);

                await _signInManager.PasswordSignInAsync(name, password, true, false);
                var customClaims = new[]
                {
                    new Claim("ProfileImage", user.Image)
                };

                await _customClaimsCookieSignInHelper.SignInUserAsync(user, true, customClaims);

                //to associate a local user account to an external login provider
                await _userManager.AddLoginAsync(user, info);
            }

            return Redirect("~/");
        }

        public IActionResult FacebookLogin()
        {
            if (Request.Cookies["Identity.External"] != null)
            {
                Response.Cookies.Delete("Identity.External");
            }

           var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", Url.Action("ExternalLoginCallback", "Auth"));
           return Challenge(properties, "Facebook");
        }
    }
}
