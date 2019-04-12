using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travelogue.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Text { get; set; }

        public string Image { get; set; }

        public int PostId { get; set; }
    }
}
