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
        private IPostRepository _postRepo;

        public TravelController(IMailService mailService, IConfigurationRoot config, 
            ILogger<TravelController> logger, IPostRepository postRepo)
        {
            _mailService = mailService;
            _config = config;
            _logger = logger;
            _postRepo = postRepo;
        }

        //[RequireHttps]
        public async Task<IActionResult> Index()
        {
            var posts = await _postRepo.GetPostsByUsername(User.Identity.Name);

            var viewModelList = new List<PostViewModel>();
            foreach (var post in posts)
            {
                var vm = new PostViewModel();
                vm.AllowsComments = post.AllowsComments;
                vm.Title = post.Title;
                vm.PostText = post.Text;
                vm.Id = post.Id;
                vm.Image = "/img/" + post.Image;

                viewModelList.Add(vm);
            }
            return View(viewModelList);
        }

        //[Authorize]
        public IActionResult Trips()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
