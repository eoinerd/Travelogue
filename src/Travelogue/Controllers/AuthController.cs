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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Microsoft.AspNetCore.Hosting;
using System.IO;
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
        public async Task<ActionResult> Register(RegisterViewModel vm, IFormFile image, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.FindByEmailAsync(vm.Email) == null)
                {
                    // use ImageWriter to upload image
                    var secureFileName = await _imageWriter.UploadImage(image);
                    var path = Path.Combine(_hostingEnvironment.WebRootPath + "\\images\\", secureFileName);

                    // Use ImageMagic (3rd party) to resize to perfect square for profile pic
                    using (Image<Rgba32> imageMagic = Image.Load(path))
                    {
                        var wideImage = imageMagic.Width - imageMagic.Height;
                        var highImage = imageMagic.Height - imageMagic.Width;

                        if (highImage > 0)
                        {
                            imageMagic.Mutate(x => x.Crop(imageMagic.Width, imageMagic.Width));
                        }
                        else if (wideImage > 0)
                        {
                            imageMagic.Mutate(x => x.Crop(imageMagic.Height, imageMagic.Height));
                        }
                        
                        imageMagic.Save(path); // Automatic encoder selected based on extension.
                    }

                    var user = new TravelUser()
                    {
                        UserName = vm.Username,
                        Email = vm.Email,
                        PhoneNumber = vm.PhoneNumber.ToString(),
                        Image = _config["ImageSettings:RootImagePath"] + secureFileName
                    };

                    var res =  await _userManager.CreateAsync(user, vm.Password);
                    
                    if (res.Errors.Any())
                        return View();

                    var signInResult = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);
                    // var user = await _userManager.FindByNameAsync(vm.Username);

                    if (signInResult.Succeeded)
                    {
                        var customClaims = new[]
                        {
                            new Claim("ProfileImage", user.Image)
                        };

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

            if (!user.Image.Contains(_config["ImageSettings:RootUrl"]))
            {
                user.Image = _config["ImageSettings:RootUrl"] + user.Image;
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
    }
}
