using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Travelogue.Data;
using Travelogue.ViewModels;
using Travelogue.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Travelogue.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int Id)
        {
            var model = _postRepository.GetPostByBlogId(Id);

            // AutoMapper
            var postViewModel = new PostViewModel();
            postViewModel.Comments = model.Comments ?? new List<Comment>();
            postViewModel.DatePosted = model.PostedOn;
            postViewModel.Title = model.Title;
            postViewModel.Post = model.Text;

            return View(postViewModel);
        }
    }
}
