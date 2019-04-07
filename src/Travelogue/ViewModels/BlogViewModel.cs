using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Travelogue.ViewModels
{
    public class BlogViewModel
    {
        public int Id { get; set; }

        [Required]
        public string BlogTitle { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool AllowsComments { get; set; }

        public string Image { get; set; }

        public List<PostViewModel> Posts { get; set; }        
    }
}
