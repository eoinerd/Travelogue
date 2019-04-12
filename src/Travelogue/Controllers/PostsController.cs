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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Travelogue.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ITravelRepository _travelRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IImageWriter _imageWriter;
        private readonly IConfigurationRoot _config;
        private readonly UserManager<TravelUser> _userManager;

        public PostsController(IPostRepository postRepository, IImageWriter imageWriter, 
            IConfigurationRoot config, ICommentRepository commentRepository, ITravelRepository travelRepository,
            UserManager<TravelUser> userManager)
        {
            _postRepository = postRepository;
            _imageWriter = imageWriter;
            _config = config;
            _commentRepository = commentRepository;
            _travelRepository = travelRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var posts = await _postRepository.GetAllPosts();

            if (!string.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Title.Contains(searchString));
            }

            var viewModelList = new List<PostViewModel>();
            foreach (var post in posts)
            {
                var vm = new PostViewModel();
                vm = Mapper.Map<PostViewModel>(post);
                vm.Image = _config["ImageSettings:RootUrl"] + post.Image;
                vm.PostText = vm.PostText.Substring(0, 150) + "....";
                viewModelList.Add(vm);
            }

            return View(viewModelList);
        }

        public async Task<IActionResult> Details(int Id)
        {
            var model = await _postRepository.GetPostById(Id);
            var postViewModel = Mapper.Map<PostViewModel>(model);
            postViewModel.Image = _config["ImageSettings:RootUrl"] + model.Image;

            return View(postViewModel);
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
                    var secureFileName = await _imageWriter.UploadImage(Image);
                    post.Image = _config["ImageSettings:RootImagePath"] + secureFileName;
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
            var result = await _postRepository.SaveChangesAsync();

            return RedirectToAction("YourPosts");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var postViewModel = new PostViewModel();

            var model = await _postRepository.GetPostById(Id);
            postViewModel = Mapper.Map<PostViewModel>(model);
            postViewModel.Image = _config["ImageSettings:RootUrl"] + model.Image;

            return View(postViewModel);
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
                postModel.Image = _config["ImageSettings:RootImagePath"] + secureFileName;
            }
            
            postModel.Published = postViewModel.Published;
            postModel.Text = postViewModel.PostText;
            postModel.UpdatedOn = DateTime.Now;
            postModel.TopTip = postViewModel.TopTip;

            _postRepository.UpdatePost(postModel);
            await _postRepository.SaveChangesAsync();

            return RedirectToAction("YourPosts");
        }

        public async Task<IActionResult> YourPosts()
        {
            var posts = await _postRepository.GetPostsByUsername(User.Identity.Name);

            var viewModelList = new List<PostViewModel>();

            foreach (var post in posts)
            {
                var vm = new PostViewModel();
                vm = Mapper.Map<PostViewModel>(post);
                vm.Image = _config["ImageSettings:RootUrl"] + post.Image;
                vm.PostText = vm.PostText.Substring(0, 150) + "....";
                viewModelList.Add(vm);
            }

            ViewBag.Message = true;
            return View("Index", viewModelList);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(Comment comment)
        {
            // email, name, posid, image
            var user = await _userManager.GetUserAsync(User);

            if (!user.Image.Contains(_config["ImageSettings:RootUrl"]))
            {
                user.Image = _config["ImageSettings:RootUrl"] + user.Image;
            }
            comment.Username = User.Identity.Name;
            comment.Image = user.Image;
            var comments = _commentRepository.AddComment(comment);
            return PartialView("_Comments", comments);
        }
    }
}
