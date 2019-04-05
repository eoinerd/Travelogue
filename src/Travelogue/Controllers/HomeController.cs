using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Travelogue.ViewModels;
using Travelogue.Services;
using Microsoft.Extensions.Configuration;
using Travelogue.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace Travelogue.Controllers
{
    public class HomeController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
       // private ITravelRepository _repository;
        private ILogger<HomeController> _logger;

        public HomeController(IMailService mailService, IConfigurationRoot config, ILogger<HomeController> logger)
        {
            _mailService = mailService;
            _config = config;
            //_repository = repository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();         
        }

       // [Authorize]
        public IActionResult Trips()
        {
            return View();
        }

        public IActionResult Blog()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
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
