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
            _context.Blogs.Add(blog);
        }

        public void DeleteBlog(int Id)
        {
            var blogToBeDeleted = _context.Blogs.Where(x => x.Id == Id).FirstOrDefault();
            _context.Blogs.Remove(blogToBeDeleted);
        }

        public async Task<IEnumerable<Blog>> GetAllBlogs()
        {
            // need to join onto Posts table here so i can
            // show the relevant Posts for each Blog
            return await _context.Blogs.ToListAsync();  //.Result.Join();
        }

        public async Task<Blog> GetBlogById(int id)
        {
            return await _context.Blogs.Include(x => x.Posts)
                                        .Where(x => x.Id == id)
                                        .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public void UpdateBlog(Blog blog)
        {
            _context.Update(blog);
        }
    }
}
