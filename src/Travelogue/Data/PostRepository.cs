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
        private StoryContext _context;

        public PostRepository(StoryContext context)
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

        public async Task<IEnumerable<Post>> GetPostsByUsername(string name)
        {
            return await _context.Posts//.Include(x => x.Posts)
                                        .Where(x => x.UserName == name)
                                        .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _context.Posts.ToListAsync();  //.Result.Join();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
