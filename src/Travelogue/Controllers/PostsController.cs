using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Travelogue.Data;
using Travelogue.ViewModels;
using Travelogue.Models;
using Microsoft.AspNetCore.Http;
using Travelogue.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Travelogue.Models.Blogs;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Travelogue.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ITravelRepository _travelRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ISubPostRepository _subPostRepository;

        private readonly IImageWriter _imageWriter;
        private readonly ILogger<PostsController> _logger;
        private readonly UserManager<TravelUser> _userManager;

        public PostsController(IPostRepository postRepository, IImageWriter imageWriter, 
             ICommentRepository commentRepository, ITravelRepository travelRepository,
            UserManager<TravelUser> userManager, ISubPostRepository subPostRepository, ILogger<PostsController> logger)
        {
            _postRepository = postRepository;
            _imageWriter = imageWriter;
            _commentRepository = commentRepository;
            _travelRepository = travelRepository;
            _userManager = userManager;
            _subPostRepository = subPostRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            var posts = await _postRepository.GetAllPosts();

            if (searchString != null)
            {
                pageNumber = 1;
            }
            //else
            //{
            //    searchString = currentFilter;
            //}

            // Basic Search 
            // NB: replace with something like Elastic Search
            if (!string.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Title.Contains(searchString));
            }

            var viewModelList = new List<PostViewModel>();
            foreach (var post in posts)
            {
                var vm = new PostViewModel();
                vm = Mapper.Map<PostViewModel>(post);
                vm.Image = "/img/" + post.Image;
                vm.PostText = vm.PostText.Length > 150 ? vm.PostText.Substring(0, 150) + "...." : vm.PostText;
                viewModelList.Add(vm);
            }

            int pageSize = 3;
            return View(PaginatedList<PostViewModel>.Create(viewModelList.AsQueryable(), pageNumber ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int Id)
        {
            try
            {
                var model = await _postRepository.GetPostById(Id);
                var postViewModel = Mapper.Map<PostViewModel>(model);
                postViewModel.Image = "/img/" + model.Image;
                postViewModel.SubPosts = await _subPostRepository.GetSubPostsByPostId(Id);

                return View(postViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Redirect("~/Home/Error");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostViewModel viewModel, IFormFile Image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var post = Mapper.Map<Post>(viewModel);
                    post.UserName = User.Identity.Name;

                    // ImageWriter to upload image for post
                    var secureFileName = await _imageWriter.UploadImage(Image);
                    post.Image = secureFileName;// _config["ImageSettings:RootImagePath"] + secureFileName;
                    post.PostedOn = DateTime.Now;
                    _postRepository.CreatePost(post);
                    await _postRepository.SaveChangesAsync();

                    // update Stops table...
                    var postForStopUpdate = _postRepository.GetPostIdByTitle(viewModel.Title);
                    _travelRepository.UpdateStop(viewModel.Trip, viewModel.Stop, postForStopUpdate.Id);
                    await _travelRepository.SaveChangesAsync();

                    return RedirectToAction("YourPosts");
                }
            }
            catch (DbUpdateException ex)
            {
                // nned to addd exception to some sort of logger here
                Console.WriteLine("Error updating: {0}", ex.Message);
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            _postRepository.DeletePost(Id);
            await _postRepository.SaveChangesAsync();

            return RedirectToAction("YourPosts");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            if (ModelState.IsValid)
            {
                var postViewModel = new PostViewModel();

                var model = await _postRepository.GetPostById(Id);
                postViewModel = Mapper.Map<PostViewModel>(model);
                postViewModel.Image = "/img/" + model.Image;

                return View(postViewModel);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PostViewModel postViewModel, IFormFile Image)
        {
            var postModel = await _postRepository.GetPostById(postViewModel.Id);

            postModel.Title = postViewModel.Title;
            postModel.AllowsComments = postViewModel.AllowsComments;

            if(Image != null)
            {
                var secureFileName = await _imageWriter.UploadImage(Image);
                postModel.Image = "/img/" + secureFileName; //_config["ImageSettings:RootImagePath"] + secureFileName;
            }
            
            postModel.Published = postViewModel.Published;
            postModel.Text = postViewModel.PostText;
            postModel.UpdatedOn = DateTime.Now;
            postModel.TopTip = postViewModel.TopTip;

            _postRepository.UpdatePost(postModel);
            await _postRepository.SaveChangesAsync();

            return RedirectToAction("YourPosts");
        }

        public async Task<IActionResult> YourPosts(int? pageNumber)
        {
            var posts = await _postRepository.GetPostsByUsername(User.Identity.Name);

            var viewModelList = new List<PostViewModel>();

            foreach (var post in posts)
            {
                var vm = new PostViewModel();
                vm = Mapper.Map<PostViewModel>(post);
                vm.Image = "/img/" + post.Image;
                vm.PostText = vm.PostText.Length > 150 ? vm.PostText.Substring(0, 150) + "...." : vm.PostText;
                viewModelList.Add(vm);
            }

            ViewBag.Message = true;
            int pageSize = 3;
            return View("Index", PaginatedList<PostViewModel>.Create(viewModelList.AsQueryable(), pageNumber ?? 1, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            // email, name, posid, image
            var user = await _userManager.GetUserAsync(User);

            if (!user.Image.Contains("/img/"))
            {
                user.Image = "/img/" + user.Image;
            }
            comment.Username = User.Identity.Name;
            comment.Image = user.Image;
            var comments = _commentRepository.AddComment(comment);
            return PartialView("Comments/_Comments", comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubPost(Models.Blogs.SubPost subPost)
        {
            if (ModelState.IsValid)
            {
                // Add/Update subPost to DB......
                if (subPost.Id > 0)
                {
                    await _subPostRepository.UpdateSubPost(subPost);
                }
                else
                {
                    await _subPostRepository.AddSubPost(subPost);
                }
                
                // Get subPosts by PostID ordered by SubPostId....
                var subPosts = await _subPostRepository.GetSubPostsByPostId(subPost.PostId);

                // Return subPosts
                return PartialView("SubPosts/_SubPosts", subPosts);
            }
            return RedirectToAction("YourPosts");
        }
    }
}
