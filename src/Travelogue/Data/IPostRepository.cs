using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;

namespace Travelogue.Data
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetPostsByBlogId(int blogId);

        void CreatePost(Post post);

        void DeletePost(Post post);

        Post GetPostByBlogId(int id);

        Task<IEnumerable<Post>> GetPostsByUsername(string username);

        Task<IEnumerable<Post>> GetAllPosts();

        Task<bool> SaveChangesAsync();
    }
}
