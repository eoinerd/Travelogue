using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;

namespace Travelogue.Data
{
    public interface IBlogRepository
    {
        Task<IEnumerable<Blog>> GetAllBlogs();

        void AddBlog(Blog blog);

        void DeleteBlog(int Id);

        Task<bool> SaveChangesAsync();

        Task<Blog> GetBlogById(int id);

        void UpdateBlog(Blog blog);
    }
}
