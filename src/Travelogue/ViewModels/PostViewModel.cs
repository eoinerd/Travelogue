using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;

namespace Travelogue.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        ////[Required]
        ////public string Location { get; set; }

        [Required]
        public string Post { get; set; }

        [Required]
        public bool AllowsComments { get; set; }

        public string Image { get; set; }

        public DateTime DatePosted { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
