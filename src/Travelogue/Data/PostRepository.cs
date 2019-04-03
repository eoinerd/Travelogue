using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;

namespace Travelogue.Data
{
    public class PostRepository : IPostRepository
    {
        private BlogContext _context;

        public PostRepository(BlogContext context)
        {
            _context = context;
        }

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
        }

        public void DeletePost(Post post)
        {
            _context.Posts.Remove(post);
        }

        public Post GetPostByBlogId(int id)
        {
            return _context.Posts.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<IEnumerable<Post>> GetPostsByBlogId(int blogId)
        {
            return await _context.Posts.ToListAsync();
        }
    }
}
