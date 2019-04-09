using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;

namespace Travelogue.Data
{
    public interface IPostRepository
    {
        Task<Post> GetPostById(int Id);

        void CreatePost(Post post);

        void DeletePost(Post post);

        Task<IEnumerable<Post>> GetPostsByUsername(string username);

        Task<IEnumerable<Post>> GetAllPosts();

        Task<bool> SaveChangesAsync();
    }
}
