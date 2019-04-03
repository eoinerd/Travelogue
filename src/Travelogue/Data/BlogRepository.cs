using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;

namespace Travelogue.Data
{
    public class BlogRepository : IBlogRepository
    {
        private BlogContext _context;

        public BlogRepository(BlogContext context)
        {
            _context = context;
        }

        public void AddBlog(Blog blog)
        {
            var g = _context.Blogs.Add(blog);
        }

        public void DeleteBlog(Blog blog)
        {
            _context.Blogs.Remove(blog);
        }

        public async Task<IEnumerable<Blog>> GetAllBlogs()
        {
            // need to join onto Posts table here so i can
            // show the relevant Posts for each Blog
            return await _context.Blogs.ToListAsync();  //.Result.Join();
        }

        public Blog GetBlogById(int id)
        {
            return _context.Blogs.Include(x => x.Posts)
                                        .Where(x => x.Id == id)
                                        .FirstOrDefault();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
