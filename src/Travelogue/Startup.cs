﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Travelogue.Services;
using Travelogue.Models;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Travelogue.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Travelogue.Data;
using Microsoft.EntityFrameworkCore;
using Travelogue.Data.Profiles;
using Travelogue.Extensions;

namespace Travelogue
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment _env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(config =>
            {
                if (_env.IsProduction())
                {
                    config.Filters.Add(new RequireHttpsAttribute());
                }
            })
            .AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            // Add functionality to inject IOptions<T>
            services.AddOptions();

            // Add our Config object so it can be injected
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.AddScoped<IMailService, DebugMailService>();
            services.AddScoped<IImageWriter, ImageWriter>();
            services.AddSingleton(Configuration);
            services.AddDbContext<TravelogueContext>();

            services.AddDbContext<StoryContext>(options => 
                options.UseSqlServer(Configuration["ConnectionStrings:Default"]));

            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ITravelRepository, TravelRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ISubPostRepository, SubPostRepository>();
            services.AddScoped<CustomClaimsCookieSignInHelper<TravelUser>>();

            services.AddIdentity<TravelUser, IdentityRole>(config =>
           {
               config.User.RequireUniqueEmail = true;
               config.Password.RequiredLength = 8;
               config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
               config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
               {
                   OnRedirectToLogin = async ctx =>
                   {
                       if (ctx.Request.Path.StartsWithSegments("/api") &&
                       ctx.Response.StatusCode == 200)
                       {
                           ctx.Response.StatusCode = 401;
                       }
                       else
                       {
                           ctx.Response.Redirect(ctx.RedirectUri);
                       }
                       await Task.Yield();
                   }
               };
           })
            .AddEntityFrameworkStores<StoryContext>();

            services.AddLogging();

            services.AddTransient<GeoCoordsService>();
            services.AddTransient<TravelContextSeedData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, TravelContextSeedData seeder)
        {
            Mapper.Initialize(Configuration =>
            {
                //Configuration.ConstructServicesUsing()
                Configuration.CreateMap<TripViewModel, Trip>().ReverseMap();
                Configuration.CreateMap<StopsViewModel, Stop>().ReverseMap();
                Configuration.AddProfile<PostProfile>();
                Configuration.AddProfile<PostViewModelProfile>();
               
            });

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();

            app.UseIdentity();
            app.UseCookieAuthentication();
            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = "419118675331691",
                AppSecret = "5084225386eca065a89e71b047cce370",
                AutomaticChallenge = true
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            seeder.EnsureSeedData().Wait();
        }
    }
}
