using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Travelogue.Controllers;
using Travelogue.Models.Blogs;

namespace Travelogue.Models
{
    public class TravelogueContext : IdentityDbContext<TravelUser>
    {
        private IConfigurationRoot _config;
        private ILogger<TravelogueContext> _logger;

        public TravelogueContext(IConfigurationRoot config, DbContextOptions options, ILogger<TravelogueContext> logger)
            :base(options)
        {
            _config = config;
            _logger = logger;
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
           // builder.UseSqlServer("Server=EOINERD\\SQLEXPRESS;Database=StoryDb;Trusted_Connection=true;MultipleActiveResultSets=true");
           _logger.LogInformation(_config["ConnectionStrings:Default"]);
            builder.UseSqlServer(_config["ConnectionStrings:Default"]);
        }
    }
}
