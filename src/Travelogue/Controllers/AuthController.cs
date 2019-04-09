using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;
using Travelogue.ViewModels;
using Travelogue.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace Travelogue.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<TravelUser> _signInManager;
        private UserManager<TravelUser> _userManager;
        private readonly IImageWriter _imageWriter;
        private readonly IConfigurationRoot _config;

        public AuthController(SignInManager<TravelUser> signInManager, UserManager<TravelUser> userManager, IImageWriter imageWriter, IConfigurationRoot config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _imageWriter = imageWriter;
            _config = config;
        }

        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Trips", "App");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel vm, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, false);

                if(signInResult.Succeeded)
                {
                    if(string.IsNullOrWhiteSpace(returnUrl))
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
                    ModelState.AddModelError("","Username or password incorrect");
                }
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel vm, IFormFile Image, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.FindByEmailAsync(vm.Email) == null)
                {
                    var secureFileName = await _imageWriter.UploadImage(Image);

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
