using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Travelogue.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Travelogue.Data;
using Travelogue.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Travelogue.Controllers
{
    public class TravelController : Controller
    {
        private IMailService _mailService;
        private IConfigurationRoot _config;
        private ILogger<TravelController> _logger;
        private IBlogRepository _blogRepo;

        public TravelController(IMailService mailService, IConfigurationRoot config, 
            ILogger<TravelController> logger, IBlogRepository blogRepo)
        {
            _mailService = mailService;
            _config = config;
            _logger = logger;
            _blogRepo = blogRepo;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _blogRepo.GetBlogsByUsername(User.Identity.Name);

            var viewModelList = new List<BlogViewModel>();
            foreach(var blog in blogs)
            {
                var vm = new BlogViewModel();
                vm.AllowsComments = blog.AllowsComments;
                vm.BlogTitle = blog.Title;
                vm.Description = blog.Subtitle;
                vm.Id = blog.Id;
                vm.Image = _config["ImageSettings:Url"] + blog.Image;

                viewModelList.Add(vm);
            }

            return View(viewModelList);
        }

        [Authorize]
        public IActionResult Trips()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
