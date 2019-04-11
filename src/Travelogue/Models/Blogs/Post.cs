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

        public DateTime UpdatedOn { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public bool AllowsComments { get; set; }

        public string UserName { get; set; }

        public string Image { get; set; }
    }
}