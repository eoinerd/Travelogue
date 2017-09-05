using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Travelogue.Models;
using Travelogue.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Travelogue.Controllers.Api
{
    public class BlogController : Controller
    {
        private readonly ITravelRepository _travelRepository;
        private readonly IList<Comment> _comments;

        public BlogController(ITravelRepository travelRepository)
        {
            _travelRepository = travelRepository;

            _comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Author = "Daniel Lo Nigro",
                    Text = "Hello ReactJS.NET World!"
                },
                new Comment
                {
                    Id = 2,
                    Author = "Pete Hunt",
                    Text = "This is one comment"
                },
                new Comment
                {
                    Id = 3,
                    Author = "Jordan Walke",
                    Text = "This is *another* comment"
                },
            };
        }

        [Route("comments")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Comments()
        {
            return Json(_comments);
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public ViewResult Posts(int p = 1)
        {
            var viewModel = new PostsViewModel(_travelRepository, p);

            ViewBag.Title = "Latest Posts";
            return View("Index", viewModel);
        }
    }
}
