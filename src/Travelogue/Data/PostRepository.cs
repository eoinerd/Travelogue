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

        public void DeletePost(int Id)
        {
            var postToBeDeleted = _context.Posts.Where(x => x.Id == Id).FirstOrDefault();
             _context.Posts.Remove(postToBeDeleted);
        }

        public async Task<Post> GetPostById(int id)
        {
            return await _context.Posts.Include(x => x.Comments).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByUsername(string name)
        {
            return await _context.Posts//.Include(x => x.Posts)
                                        .Where(x => x.UserName == name)
                                        .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await _context.Posts.Where(x => x.Published).ToListAsync();  //.Result.Join();
        }

        public void UpdatePost(Post post)
        {
            _context.Update(post);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public Post GetPostIdByTitle(string title)
        {
            return _context.Posts.Where(x => x.Title == title).FirstOrDefault();
        }
    }
}
