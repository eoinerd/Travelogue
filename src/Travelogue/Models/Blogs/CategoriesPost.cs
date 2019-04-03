using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Travelogue.Models.Blogs
{
    public class CategoriesPost
    {
        public int CategoriesPostId { get; set; }

        public int CategoryId { get; set; }

        public int PostId { get; set; }
    }
}
