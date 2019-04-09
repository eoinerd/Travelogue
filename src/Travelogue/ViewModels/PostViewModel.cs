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
        [StringLength(6000, MinimumLength = 301)]
        public string Post { get; set; }

        [Required]
        public bool AllowsComments { get; set; }

        public string Image { get; set; }

        public string Stop { get; set; }

        public string Trip { get; set; }

        public bool Published { get; set; }

        public DateTime DatePosted { get; set; }

        public DateTime DateUpdated { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
