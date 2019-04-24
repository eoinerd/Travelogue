namespace Travelogue.Models.Blogs
{
    public class SubPost
    {
        public int Id { get; set; }

        public string Category { get; set; }

        public string SubPostText { get; set; }

        public int PostId { get; set; }
    }
}