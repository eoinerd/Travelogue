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

        void DeleteBlog(Blog blog);

        Task<bool> SaveChangesAsync();

        Blog GetBlogById(int id);
    }
}
