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

        public IEnumerable<Comment> AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();

            return _context.Comments.Where(x => x.PostId == comment.PostId).ToList();
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostId(int postId)
        {
            var hasComments = _context.Comments.Any(x => x.PostId == postId);
            if (!hasComments)
            {
                return new List<Comment>();
            }
            return await  _context.Comments.Where(x => x.PostId == postId).ToListAsync();
        }
    }
}
