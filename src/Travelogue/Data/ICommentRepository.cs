using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;

namespace Travelogue.Data
{
    public interface ICommentRepository
    {
        void AddComment(Comment comment);

        Task<IEnumerable<Comment>> GetCommentsByPostId(int postId);
    }
}
