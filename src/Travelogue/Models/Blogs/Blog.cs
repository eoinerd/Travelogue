using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travelogue.Models
{
    public class Blog
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public bool AllowsComments { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
