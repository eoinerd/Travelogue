using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models.Blogs;

namespace Travelogue.Models
{
    public class Blog
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public bool AllowsComments { get; set; }

        public DateTime CreatedAt { get; set; }

        public string UserName { get; set; }

        public ICollection<Post> Posts { get; set; }

        public string Image { get; set; }
    }
}
