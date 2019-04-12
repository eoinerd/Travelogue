using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;

namespace Travelogue.Data
{
    public interface ICommentRepository
    {
        IEnumerable<Comment> AddComment(Comment comment);

        Task<IEnumerable<Comment>> GetCommentsByPostId(int postId);
    }
}
