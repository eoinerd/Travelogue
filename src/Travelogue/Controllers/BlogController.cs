using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Travelogue.Models;
using Travelogue.ViewModels;
using Travelogue.Data;
using Microsoft.EntityFrameworkCore;

namespace Travelogue.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IList<Comment> _comments;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _blogRepository.GetAllBlogs();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogViewModel model)
        {
            if (ModelState.IsValid)
            {
                // should use automapper here.....
                var blog = new Blog() {};
                blog.AllowsComments = model.AllowsComments;
                blog.CreatedAt = DateTime.Now;
                blog.Subtitle = model.Description;
                blog.Title = model.BlogTitle;

                // need exception handling here....
                _blogRepository.AddBlog(blog);

                await _blogRepository.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpGet("")]
        public IActionResult Details(int Id)
        {
            var blogViewModel = new BlogViewModel();

            try
            {
                // automapper
                var model = _blogRepository.GetBlogById(Id);
                blogViewModel.AllowsComments = model.AllowsComments;
                blogViewModel.BlogTitle = model.Title;
                blogViewModel.Description = model.Subtitle;
                blogViewModel.Posts = new List<PostViewModel>();

                // automapper
                foreach (var post in model.Posts)
                {
                    var postViewModel = new PostViewModel();
                    postViewModel.Id = post.Id;
                    postViewModel.DatePosted = post.PostedOn;
                    postViewModel.Post = post.Text;
                    postViewModel.Title = post.Title;
                    postViewModel.Comments = post.Comments ?? new List<Comment>();
                    blogViewModel.Posts.Add(postViewModel);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception {0}", ex.Message);
            }

            return View("Details", blogViewModel);
        }

        //[Route("comments")]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        //public ActionResult Comments()
        //{
        //    return Json(_comments);
        //}

        //public ViewResult Posts(int p = 1)
        //{
        //    var viewModel = new PostsViewModel(_travelRepository, p);

        //    ViewBag.Title = "Latest Posts";
        //    return View("Index", viewModel);
        //}

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Create(PostViewModel model)
        //{
        //    ////// pass view model to a call that will use automapper 
        //    ////// to convert to Post model and save to DB
        //    ////if (ModelState.IsValid)
        //    ////{
        //    ////    _travelRepository.SavePost(model);
        //    ////}

        //    return RedirectToAction("Posts");
        //}
    }
}
