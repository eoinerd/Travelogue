using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Travelogue.Models
{
    public class UsersBlog
    {
        public int UsersBlogId { get; set; }

        public int UserId { get; set; }

        public int BlogId { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}
