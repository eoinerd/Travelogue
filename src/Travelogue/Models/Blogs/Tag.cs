using System.Collections.Generic;

namespace Travelogue.Models
{
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ItemId { get; set; }

        public string ItemType { get; set; }
    }
}