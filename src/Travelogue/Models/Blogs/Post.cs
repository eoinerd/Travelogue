using System;
using System.Collections.Generic;

namespace Travelogue.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public bool Published { get; set; }

        public DateTime PostedOn { get; set; }

        public int UserId { get; set; }

        public int BlogId { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}