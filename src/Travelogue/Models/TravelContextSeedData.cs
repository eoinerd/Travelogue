﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Data;
using Travelogue.Models.Blogs;

namespace Travelogue.Models
{
    public class TravelContextSeedData
    {
        private StoryContext _context;
        private UserManager<TravelUser> _userManager;

        public TravelContextSeedData(StoryContext context, UserManager<TravelUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task EnsureSeedData()
        {
            _context.Database.EnsureCreated();

            if (await _userManager.FindByEmailAsync("eoinerd@gmail.com") == null)
            {
                var user = new TravelUser()
                {
                    UserName = "eoiner",
                    Email = "eoinerd@gmail.com",
                    Image = "/images/dffbc8b5-44ac-4b73-af11-aaa23b613ddb.jpg"
                };

                await _userManager.CreateAsync(user, "Fiatpunto26%20");
            }

            // Look for any students.
            if (_context.Posts.Any())
            {
                return;   // DB has been seeded
            }

            var posts = new Post[]
            {
                new Post{Title="Siem Reap", Text="On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain.These cases are perfectly simple and easy to distinguish. In a free hour, when our power of choice is untrammelled and when nothing prevents our being able to do what we like best, every pleasure is to be welcomed and every pain avoided. But in certain circumstances and owing to the claims of duty or the obligations of business it will frequently occur that pleasures have to be repudiated and annoyances accepted.The wise man therefore always holds in these matters to this principle of selection: he rejects pleasures to secure other greater pleasures, or else he endures pains to avoid worse pains.", PostedOn=DateTime.Now, UpdatedOn=DateTime.Now, Published=true, UserName ="eoiner", Image="images/IMG_20190227_144127.jpg"},
                new Post{Title="Koh Rong", Text="At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat.", PostedOn=DateTime.Now, UpdatedOn=DateTime.Now, Published=true, UserName ="eoiner", Image="images/IMG_20190310_135551.jpg"},
                new Post{Title="Phnom Penh", Text="But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes, or avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?", PostedOn=DateTime.Now, UpdatedOn=DateTime.Now, Published=true, UserName ="eoiner", Image="images/IMG-20181213-WA0003.jpg"}
            };
            foreach (Post p in posts)
            {
                _context.Posts.Add(p);
            }
            _context.SaveChanges();

            // Look for any students.
            if (_context.SubPosts.Any())
            {
                return;   // DB has been seeded
            }

            var subPosts = new SubPost[]
            {
                new SubPost {Category = "Food", SubPostText = "some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue some stuff about food in some place becasue ", PostId = 1} , 
            };
            foreach (SubPost sp in subPosts)
            {
                _context.SubPosts.Add(sp);
            }
            _context.SaveChanges();

            if (!_context.Trips.Any())
            {
                var usTrip = new Trip()
                {
                    DateCreated = DateTime.UtcNow,
                    Name = "US Trip",
                    UserName = "eoiner",
                    Stops = new List<Stop>()
                    {
                        new Stop() {  Name = "Atlanta, GA", ArrivalDate = new DateTime(2014, 6, 4), Latitude = 33.748995, Longitude = -84.387982, Order = 0, PostId=0 },
                        new Stop() {  Name = "New York, NY", ArrivalDate = new DateTime(2014, 6, 9), Latitude = 40.712784, Longitude = -74.005941, Order = 1, PostId=0  },
                        new Stop() {  Name = "Boston, MA", ArrivalDate = new DateTime(2014, 7, 1), Latitude = 42.360082, Longitude = -71.058880, Order = 2, PostId=0  },
                        new Stop() {  Name = "Chicago, IL", ArrivalDate = new DateTime(2014, 7, 10), Latitude = 41.878114, Longitude = -87.629798, Order = 3, PostId=0  },
                        new Stop() {  Name = "Seattle, WA", ArrivalDate = new DateTime(2014, 8, 13), Latitude = 47.606209, Longitude = -122.332071, Order = 4, PostId=0  },
                        new Stop() {  Name = "Atlanta, GA", ArrivalDate = new DateTime(2014, 8, 23), Latitude = 33.748995, Longitude = -84.387982, Order = 5, PostId=0  },
                    }
                };

                _context.Trips.Add(usTrip);
                _context.Stops.AddRange(usTrip.Stops);

                var worldStop = new Trip()
                {
                    DateCreated = DateTime.UtcNow,
                    Name = "WorldTrip",
                    UserName = "eoiner",
                    Stops = new List<Stop>()
                    {
                        new Stop() { Order = 0, Latitude =  33.748995, Longitude =  -84.387982, Name = "Atlanta, Georgia", ArrivalDate = DateTime.Parse("Jun 3, 2014") },
                        new Stop() { Order = 1, Latitude =  48.856614, Longitude =  2.352222, Name = "Paris, France", ArrivalDate = DateTime.Parse("Jun 4, 2014"), PostId=0  },
                        new Stop() { Order = 2, Latitude =  50.850000, Longitude =  4.350000, Name = "Brussels, Belgium", ArrivalDate = DateTime.Parse("Jun 25, 2014"), PostId=0  },
                        new Stop() { Order = 3, Latitude =  51.209348, Longitude =  3.224700, Name = "Bruges, Belgium", ArrivalDate = DateTime.Parse("Jun 28, 2014"), PostId=0  },
                        new Stop() { Order = 4, Latitude =  53.349805, Longitude =  -6.260310, Name = "Dublin, Ireland", ArrivalDate = DateTime.Parse("Sep 2, 2014"), PostId=0  },
                        new Stop() { Order = 5, Latitude =  54.597285, Longitude =  -5.930120, Name = "Belfast, Northern Ireland", ArrivalDate = DateTime.Parse("Sep 7, 2014"), PostId=0  },
                        new Stop() { Order = 6, Latitude =  41.005270, Longitude =  28.976960, Name = "Istanbul, Turkey", ArrivalDate = DateTime.Parse("Nov 15, 2014"), PostId=0  },
                        new Stop() { Order = 36, Latitude =  50.937531, Longitude =  6.960279, Name = "Cologne, Germany", ArrivalDate = DateTime.Parse("Nov 30, 2014"), PostId=0  }
                    }
                };

                _context.Trips.Add(worldStop);
                _context.Stops.AddRange(worldStop.Stops);

                await _context.SaveChangesAsync();
            }
        }
    }
}
