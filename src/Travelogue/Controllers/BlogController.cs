using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Travelogue.Models;
using Travelogue.ViewModels;
using Travelogue.Data;
using Microsoft.EntityFrameworkCore;
using Travelogue.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Travelogue.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IList<Comment> _comments;
        private readonly IImageWriter _imageWriter;
        private readonly IConfigurationRoot _config;

        public BlogController(IBlogRepository blogRepository, IImageWriter imageWriter, IConfigurationRoot config)
        {
            _blogRepository = blogRepository;
            _imageWriter = imageWriter;
            _config = config;
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
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogViewModel viewModel, IFormFile Image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // should use automapper here.....
                    var blog = new Blog();
                    blog.AllowsComments = viewModel.AllowsComments;
                    blog.CreatedAt = DateTime.Now;
                    blog.Subtitle = viewModel.Description;
                    blog.Title = viewModel.BlogTitle;
                    blog.UserName = User.Identity.Name;
                    var secureFileName = await _imageWriter.UploadImage(Image);
                    blog.Image = _config["ImageSettings:RootImagePath"] + secureFileName;
                    // need exception handling here....
                    _blogRepository.AddBlog(blog);

                    await _blogRepository.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch(DbUpdateException ex)
            {
                // nned to addd exception to some sort of logger here
                Console.WriteLine("Error updating: {0}", ex.Message);
            }

            return View(viewModel);
        }

        [HttpGet("")]
        public async Task<IActionResult> Details(int Id)
        {
            var blogViewModel = new BlogViewModel();

            try
            {
                // automapper
                var model = await _blogRepository.GetBlogById(Id);
                blogViewModel.AllowsComments = model.AllowsComments;
                blogViewModel.BlogTitle = model.Title;
                blogViewModel.Description = model.Subtitle;
                blogViewModel.Id = model.Id;
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

            return View(blogViewModel);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            _blogRepository.DeleteBlog(Id);
            var result = await _blogRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var viewModel = new BlogViewModel();

            var blogModel = await _blogRepository.GetBlogById(Id);

            // automapper needed
            viewModel.AllowsComments = blogModel.AllowsComments;
            viewModel.BlogTitle = blogModel.Title;
            viewModel.Description = blogModel.Subtitle;
            viewModel.Id = Id;
            viewModel.Posts = new List<PostViewModel>();

            // automapper
            foreach (var post in blogModel.Posts)
            {
                var postViewModel = new PostViewModel();
                postViewModel.Id = post.Id;
                postViewModel.DatePosted = post.PostedOn;
                postViewModel.Post = post.Text;
                postViewModel.Title = post.Title;
                postViewModel.Comments = post.Comments ?? new List<Comment>();
                viewModel.Posts.Add(postViewModel);
            }
            return View(viewModel);
        }
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BlogViewModel blogViewModel)
        {
            var blogModel = await _blogRepository.GetBlogById(blogViewModel.Id);

            blogModel.Title = blogViewModel.BlogTitle;
            blogModel.AllowsComments = blogViewModel.AllowsComments;
            blogModel.Subtitle = blogViewModel.Description;

            _blogRepository.UpdateBlog(blogModel);
            await _blogRepository.SaveChangesAsync();

            return RedirectToAction("Index");
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
