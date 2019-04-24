using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Travelogue.ViewModels;
using SubPost = Travelogue.Models.Blogs.SubPost;

namespace Travelogue.Data
{
    public class SubPostRepository : ISubPostRepository
    {
        private StoryContext _context;

        public SubPostRepository(StoryContext context)
        {
            _context = context;
        }

        public async Task AddSubPost(SubPost subPost)
        {
            _context.SubPosts.Add(subPost);
            await SaveChangesAsync();
        }

        public async Task UpdateSubPost(SubPost subPost)
        {
            _context.SubPosts.Update(subPost);
            await SaveChangesAsync();
        }

        public async Task<IEnumerable<SubPost>> GetSubPostsByPostId(int postId)
        {
            return await _context.SubPosts.Where(x => x.PostId == postId).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
