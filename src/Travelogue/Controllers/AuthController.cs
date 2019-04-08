using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;
using Travelogue.ViewModels;

namespace Travelogue.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<TravelUser> _signInManager;
        private UserManager<TravelUser> _userManager;

        public AuthController(SignInManager<TravelUser> signInManager, UserManager<TravelUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
        public async Task<ActionResult> Register(RegisterViewModel vm, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (await _userManager.FindByEmailAsync(vm.Email) == null)
                {
                    var user = new TravelUser()
                    {
                        UserName = vm.Username,
                        Email = vm.Email,
                        PhoneNumber = vm.PhoneNumber.ToString()
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
