using System;
using System.Collections.Generic;

namespace Travelogue.Models
{
    public class Post
    {
        public int Id
        { get; set; }

        public string Title
        { get; set; }

        public string ShortDescription
        { get; set; }

        public string Description
        { get; set; }

        public string Meta
        { get; set; }

        public string UrlSlug
        { get; set; }

        public bool Published
        { get; set; }

        public DateTime PostedOn
        { get; set; }

        public DateTime? Modified
        { get; set; }

        public Category Category
        { get; set; }

        //public IList<Tag> Tags
        //{ get; set; }
    }
}