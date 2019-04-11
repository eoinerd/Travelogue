﻿using System;
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

        public PostsController(IPostRepository postRepository, IImageWriter imageWriter, 
            IConfigurationRoot config, ICommentRepository commentRepository, ITravelRepository travelRepository)
        {
            _postRepository = postRepository;
            _imageWriter = imageWriter;
            _config = config;
            _commentRepository = commentRepository;
            _travelRepository = travelRepository;
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
                vm.AllowsComments = post.AllowsComments;
                vm.Title = post.Title;
                vm.PostedOn = post.PostedOn;
                vm.Text = post.Text.Substring(0, 300) + "....";
                vm.Id = post.Id;               
                vm.Image = _config["ImageSettings:RootUrl"] + post.Image;

                viewModelList.Add(vm);
            }

            return View(viewModelList);
        }

        public async Task<IActionResult> Details(int Id)
        {
            var model = await _postRepository.GetPostById(Id);

            // AutoMapper
            var postViewModel = new PostViewModel();
            postViewModel.Comments = model.Comments ?? new List<Comment>();
            postViewModel.PostedOn = model.PostedOn;
            postViewModel.Title = model.Title;
            postViewModel.Text = model.Text;
            postViewModel.Id = model.Id;
            postViewModel.Comments = await _commentRepository.GetCommentsByPostId(model.Id);
            postViewModel.Image = _config["ImageSettings:RootUrl"] + model.Image;

            return View(postViewModel);
        }

        public IActionResult Create([FromQuery(Name = "trip")] string trip, [FromQuery(Name = "stop")] string stop)
        {
            var test = trip;
            return View();
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
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
                
                    // need exception handling here....
                    _postRepository.CreatePost(post);
                    await _postRepository.SaveChangesAsync();

                    // update Stops table...
                    var postForStopUpdate = _postRepository.GetPostIdByTitle(viewModel.Title);
                    _travelRepository.UpdateStop(viewModel.Trip, viewModel.Stop, postForStopUpdate.Id);
                    await _travelRepository.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
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

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var postViewModel = new PostViewModel();

            var model = await _postRepository.GetPostById(Id);

            // automapper needed
            postViewModel.Comments = model.Comments ?? new List<Comment>();
            postViewModel.PostedOn = model.PostedOn;
            postViewModel.AllowsComments = model.AllowsComments;
            postViewModel.Title = model.Title;
            postViewModel.Text = model.Text;
            postViewModel.Published = model.Published;
            postViewModel.Id = model.Id;
            postViewModel.Image = _config["ImageSettings:RootUrl"] + model.Image;

            return View(postViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
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
            postModel.Text = postViewModel.Text;

            _postRepository.UpdatePost(postModel);
            await _postRepository.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> YourPosts()
        {
            var posts = await _postRepository.GetPostsByUsername(User.Identity.Name);

            var viewModelList = new List<PostViewModel>();

            foreach (var post in posts)
            {
                var vm = new PostViewModel();
                vm.AllowsComments = post.AllowsComments;
                vm.Title = post.Title;
                vm.PostedOn = post.PostedOn;
                vm.Text = post.Text.Substring(0, 300) + "....";
                vm.Id = post.Id;
                vm.Image = _config["ImageSettings:RootUrl"] + post.Image;

                viewModelList.Add(vm);
            }

            return View("Index", viewModelList);
        }
    }
}
