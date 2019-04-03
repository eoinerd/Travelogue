using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;

namespace Travelogue.Data
{
    public class DbInitializer
    {
        public static void Initialize(BlogContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Blogs.Any())
            {
                return;   // DB has been seeded
            }

            var blogs = new Blog[]
            {
                new Blog{Title="Cambodia", Subtitle="A month in a cool country", AllowsComments=true, CreatedAt=DateTime.Now}
            };
            foreach (Blog b in blogs)
            {
                context.Blogs.Add(b);
            }
            context.SaveChanges();

            var posts = new Post[]
            {
                new Post{Title="Siem Reap", Text="blah blah blah", BlogId=1, PostedOn=DateTime.Now, Published=true, UserId=1},
                new Post{Title="Koh Rong", Text="boob obobobob", BlogId=1, PostedOn=DateTime.Now, Published=true, UserId=1},
                new Post{Title="Phnom Penh", Text="asdasdfasdasf asdasd", BlogId=1, PostedOn=DateTime.Now, Published=true, UserId=1}
            };
            foreach (Post p in posts)
            {
                context.Posts.Add(p);
            }
            context.SaveChanges();
        }
    }
}
