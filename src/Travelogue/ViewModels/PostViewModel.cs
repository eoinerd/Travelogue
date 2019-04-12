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

        [Required]
        [StringLength(6000, MinimumLength = 151)]
        public string PostText { get; set; }

        [Required]
        public bool AllowsComments { get; set; }

        public string Image { get; set; }

        public bool Published { get; set; }

        public DateTime PostedOn { get; set; }

        public DateTime DateUpdated { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public string Trip { get; set; }

        public string Stop { get; set; }

        public string Username { get; set; }
    }
}
