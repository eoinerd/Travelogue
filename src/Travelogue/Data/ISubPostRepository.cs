using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SubPost = Travelogue.Models.Blogs.SubPost;

namespace Travelogue.Data
{
    public interface ISubPostRepository
    {
        Task AddSubPost(SubPost subPost);

        Task UpdateSubPost(SubPost subPost);

        Task<IEnumerable<SubPost>> GetSubPostsByPostId(int postId);

        Task<bool> SaveChangesAsync();
    }
}
