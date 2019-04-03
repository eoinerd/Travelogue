using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Travelogue.Models;
using Travelogue.Models.Blogs;

namespace Travelogue.Data
{
    public class BlogContext : DbContext
    {
        private IConfigurationRoot _config;

        public BlogContext(DbContextOptions<BlogContext> options, IConfigurationRoot config) : base(options)
        {
            _config = config;
        }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<CategoriesPost> CategoriesPosts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<UsersBlog> UsersBlogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>().ToTable("Blogs");
            modelBuilder.Entity<Post>().ToTable("Posts");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<UsersBlog>().ToTable("UsersBlogs");
            modelBuilder.Entity<Tag>().ToTable("Tags");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<CategoriesPost>().ToTable("CategoriesPosts");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);

            builder.UseSqlServer(_config["ConnectionStrings:BlogContextConnection"]);
        }
    }
}
