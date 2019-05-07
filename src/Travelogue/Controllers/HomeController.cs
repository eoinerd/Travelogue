using Microsoft.AspNetCore.Mvc;
using Travelogue.ViewModels;
using Travelogue.Services;
using Microsoft.Extensions.Configuration;
using Travelogue.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace Travelogue.Controllers
{
    public class HomeController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private ILogger<HomeController> _logger;
        private UserManager<TravelUser> _userManager;

        public HomeController(IMailService mailService, IConfigurationRoot config, ILogger<HomeController> logger)
        {
            _mailService = mailService;
            _config = config;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToRoute(new
            {
                controller = "Travel",
                action = "Index"
            });
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if(ModelState.IsValid)
            {
                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "From Travelogue", model.Message);
                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent";
            }

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
