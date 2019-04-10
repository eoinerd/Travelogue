using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;

namespace Travelogue.Data
{
    public class CommentRepository : ICommentRepository
    {
        private StoryContext _context;

        public CommentRepository(StoryContext context)
        {
            _context = context;
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
        {
            return await _context.Comments.Where(x => x.PostId == postId).ToListAsync();
        }
    }
}
