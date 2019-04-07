using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Data;

namespace Travelogue.Models
{
    public class TravelContextSeedData
    {
        private BlogContext _context;
        private UserManager<TravelUser> _userManager;

        public TravelContextSeedData(BlogContext context, UserManager<TravelUser> userManager)
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
                    Email = "eoinerd@gmail.com"
                };

                await _userManager.CreateAsync(user, "Fiatpunto26%20");
            }

            // Look for any students.
            if (_context.Blogs.Any())
            {
                return;   // DB has been seeded
            }

            var blogs = new Blog[]
            {
                new Blog{ Title="Cambodia", Subtitle="A month in a cool country", AllowsComments=true, CreatedAt=DateTime.Now,
                    UserName ="eoiner", Image="C:\\Users\\Eoin\\Documents\\Visual Studio 2015\\Projects\\Travelogue\\Travelogue\\src\\Travelogue\\wwwroot\\images\\IMG-20181031-WA0000.jpg"}
            };
            foreach (Blog b in blogs)
            {
                _context.Blogs.Add(b);
            }
            _context.SaveChanges();

            var posts = new Post[]
            {
                new Post{Title="Siem Reap", Text="blah blah blah", BlogId=1, PostedOn=DateTime.Now, Published=true, UserId=1},
                new Post{Title="Koh Rong", Text="boob obobobob", BlogId=1, PostedOn=DateTime.Now, Published=true, UserId=1},
                new Post{Title="Phnom Penh", Text="asdasdfasdasf asdasd", BlogId=1, PostedOn=DateTime.Now, Published=true, UserId=1}
            };
            foreach (Post p in posts)
            {
                _context.Posts.Add(p);
            }

            if (!_context.Trips.Any())
            {
                var usTrip = new Trip()
                {
                    DateCreated = DateTime.UtcNow,
                    Name = "US Trip",
                    UserName = "eoiner",
                    Stops = new List<Stop>()
                    {
                        new Stop() {  Name = "Atlanta, GA", ArrivalDate = new DateTime(2014, 6, 4), Latitude = 33.748995, Longitude = -84.387982, Order = 0 },
                        new Stop() {  Name = "New York, NY", ArrivalDate = new DateTime(2014, 6, 9), Latitude = 40.712784, Longitude = -74.005941, Order = 1 },
                        new Stop() {  Name = "Boston, MA", ArrivalDate = new DateTime(2014, 7, 1), Latitude = 42.360082, Longitude = -71.058880, Order = 2 },
                        new Stop() {  Name = "Chicago, IL", ArrivalDate = new DateTime(2014, 7, 10), Latitude = 41.878114, Longitude = -87.629798, Order = 3 },
                        new Stop() {  Name = "Seattle, WA", ArrivalDate = new DateTime(2014, 8, 13), Latitude = 47.606209, Longitude = -122.332071, Order = 4 },
                        new Stop() {  Name = "Atlanta, GA", ArrivalDate = new DateTime(2014, 8, 23), Latitude = 33.748995, Longitude = -84.387982, Order = 5 },
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
            new Stop() { Order = 1, Latitude =  48.856614, Longitude =  2.352222, Name = "Paris, France", ArrivalDate = DateTime.Parse("Jun 4, 2014") },
            new Stop() { Order = 2, Latitude =  50.850000, Longitude =  4.350000, Name = "Brussels, Belgium", ArrivalDate = DateTime.Parse("Jun 25, 2014") },
            new Stop() { Order = 3, Latitude =  51.209348, Longitude =  3.224700, Name = "Bruges, Belgium", ArrivalDate = DateTime.Parse("Jun 28, 2014") },
            new Stop() { Order = 4, Latitude =  48.856614, Longitude =  2.352222, Name = "Paris, France", ArrivalDate = DateTime.Parse("Jun 30, 2014") },
            new Stop() { Order = 5, Latitude =  51.508515, Longitude =  -0.125487, Name = "London, UK", ArrivalDate = DateTime.Parse("Jul 8, 2014") },
            new Stop() { Order = 6, Latitude =  51.454513, Longitude =  -2.587910, Name = "Bristol, UK", ArrivalDate = DateTime.Parse("Jul 24, 2014") },
            new Stop() { Order = 7, Latitude =  52.078000, Longitude =  -2.783000, Name = "Stretton Sugwas, UK", ArrivalDate = DateTime.Parse("Jul 29, 2014") },
            new Stop() { Order = 8, Latitude =  51.864211, Longitude =  -2.238034, Name = "Gloucestershire, UK", ArrivalDate = DateTime.Parse("Jul 30, 2014") },
            new Stop() { Order = 9, Latitude =  52.954783, Longitude =  -1.158109, Name = "Nottingham, UK", ArrivalDate = DateTime.Parse("Jul 31, 2014") },
            new Stop() { Order = 10, Latitude =  51.508515, Longitude =  -0.125487, Name = "London, UK", ArrivalDate = DateTime.Parse("Aug 1, 2014") },
            new Stop() { Order = 11, Latitude =  55.953252, Longitude =  -3.188267, Name = "Edinburgh, UK", ArrivalDate = DateTime.Parse("Aug 5, 2014") },
            new Stop() { Order = 12, Latitude =  55.864237, Longitude =  -4.251806, Name = "Glasgow, UK", ArrivalDate = DateTime.Parse("Aug 6, 2014") },
            new Stop() { Order = 13, Latitude =  57.149717, Longitude =  -2.094278, Name = "Aberdeen, UK", ArrivalDate = DateTime.Parse("Aug 7, 2014") },
            new Stop() { Order = 14, Latitude =  55.953252, Longitude =  -3.188267, Name = "Edinburgh, UK", ArrivalDate = DateTime.Parse("Aug 8, 2014") },
            new Stop() { Order = 15, Latitude =  51.508515, Longitude =  -0.125487, Name = "London, UK", ArrivalDate = DateTime.Parse("Aug 10, 2014") },
            new Stop() { Order = 16, Latitude =  52.370216, Longitude =  4.895168, Name = "Amsterdam, Netherlands", ArrivalDate = DateTime.Parse("Aug 14, 2014") },
            new Stop() { Order = 17, Latitude =  48.583148, Longitude =  7.747882, Name = "Strasbourg, France", ArrivalDate = DateTime.Parse("Aug 17, 2014") },
            new Stop() { Order = 18, Latitude =  46.519962, Longitude =  6.633597, Name = "Lausanne, Switzerland", ArrivalDate = DateTime.Parse("Aug 19, 2014") },
            new Stop() { Order = 19, Latitude =  46.021073, Longitude =  7.747937, Name = "Zermatt, Switzerland", ArrivalDate = DateTime.Parse("Aug 27, 2014") },
            new Stop() { Order = 20, Latitude =  46.519962, Longitude =  6.633597, Name = "Lausanne, Switzerland", ArrivalDate = DateTime.Parse("Aug 29, 2014") },
            new Stop() { Order = 21, Latitude =  53.349805, Longitude =  -6.260310, Name = "Dublin, Ireland", ArrivalDate = DateTime.Parse("Sep 2, 2014") },
            new Stop() { Order = 22, Latitude =  54.597285, Longitude =  -5.930120, Name = "Belfast, Northern Ireland", ArrivalDate = DateTime.Parse("Sep 7, 2014") },
            new Stop() { Order = 23, Latitude =  53.349805, Longitude =  -6.260310, Name = "Dublin, Ireland", ArrivalDate = DateTime.Parse("Sep 9, 2014") },
            new Stop() { Order = 24, Latitude =  47.368650, Longitude =  8.539183, Name = "Zurich, Switzerland", ArrivalDate = DateTime.Parse("Sep 16, 2014") },
            new Stop() { Order = 25, Latitude =  48.135125, Longitude =  11.581981, Name = "Munich, Germany", ArrivalDate = DateTime.Parse("Sep 19, 2014") },
            new Stop() { Order = 26, Latitude =  50.075538, Longitude =  14.437800, Name = "Prague, Czech Republic", ArrivalDate = DateTime.Parse("Sep 21, 2014") },
            new Stop() { Order = 27, Latitude =  51.050409, Longitude =  13.737262, Name = "Dresden, Germany", ArrivalDate = DateTime.Parse("Oct 1, 2014") },
            new Stop() { Order = 28, Latitude =  50.075538, Longitude =  14.437800, Name = "Prague, Czech Republic", ArrivalDate = DateTime.Parse("Oct 4, 2014") },
            new Stop() { Order = 29, Latitude =  42.650661, Longitude =  18.094424, Name = "Dubrovnik, Croatia", ArrivalDate = DateTime.Parse("Oct 10, 2014") },
            new Stop() { Order = 30, Latitude =  42.697708, Longitude =  23.321868, Name = "Sofia, Bulgaria", ArrivalDate = DateTime.Parse("Oct 16, 2014") },
            new Stop() { Order = 31, Latitude =  45.658928, Longitude =  25.539608, Name = "Brosov, Romania", ArrivalDate = DateTime.Parse("Oct 20, 2014") },
            new Stop() { Order = 32, Latitude =  41.005270, Longitude =  28.976960, Name = "Istanbul, Turkey", ArrivalDate = DateTime.Parse("Nov 1, 2014") },
            new Stop() { Order = 33, Latitude =  45.815011, Longitude =  15.981919, Name = "Zagreb, Croatia", ArrivalDate = DateTime.Parse("Nov 11, 2014") },
            new Stop() { Order = 34, Latitude =  41.005270, Longitude =  28.976960, Name = "Istanbul, Turkey", ArrivalDate = DateTime.Parse("Nov 15, 2014") },
            new Stop() { Order = 35, Latitude =  50.850000, Longitude =  4.350000, Name = "Brussels, Belgium", ArrivalDate = DateTime.Parse("Nov 25, 2014") },
            new Stop() { Order = 36, Latitude =  50.937531, Longitude =  6.960279, Name = "Cologne, Germany", ArrivalDate = DateTime.Parse("Nov 30, 2014") },
            new Stop() { Order = 37, Latitude =  48.208174, Longitude =  16.373819, Name = "Vienna, Austria", ArrivalDate = DateTime.Parse("Dec 4, 2014") },
            new Stop() { Order = 38, Latitude =  47.497912, Longitude =  19.040235, Name = "Budapest, Hungary", ArrivalDate = DateTime.Parse("Dec 28,2014") },
            new Stop() { Order = 39, Latitude =  37.983716, Longitude =  23.729310, Name = "Athens, Greece", ArrivalDate = DateTime.Parse("Jan 2, 2015") },
            new Stop() { Order = 40, Latitude =  -25.746111, Longitude =  28.188056, Name = "Pretoria, South Africa", ArrivalDate = DateTime.Parse("Jan 19, 2015") },
            new Stop() { Order = 41, Latitude =  43.771033, Longitude =  11.248001, Name = "Florence, Italy", ArrivalDate = DateTime.Parse("Feb 1, 2015") },
            new Stop() { Order = 42, Latitude =  45.440847, Longitude =  12.315515, Name = "Venice, Italy", ArrivalDate = DateTime.Parse("Feb 9, 2015") },
            new Stop() { Order = 43, Latitude =  43.771033, Longitude =  11.248001, Name = "Florence, Italy", ArrivalDate = DateTime.Parse("Feb 13, 2015") },
            new Stop() { Order = 44, Latitude =  41.872389, Longitude =  12.480180, Name = "Rome, Italy", ArrivalDate = DateTime.Parse("Feb 17, 2015") },
            new Stop() { Order = 45, Latitude =  28.632244, Longitude =  77.220724, Name = "New Delhi, India", ArrivalDate = DateTime.Parse("Mar 4, 2015") },
            new Stop() { Order = 46, Latitude =  27.700000, Longitude =  85.333333, Name = "Kathmandu, Nepal", ArrivalDate = DateTime.Parse("Mar 10, 2015") },
            new Stop() { Order = 47, Latitude =  28.632244, Longitude =  77.220724, Name = "New Delhi, India", ArrivalDate = DateTime.Parse("Mar 11, 2015") },
            new Stop() { Order = 48, Latitude =  22.1667, Longitude =  113.5500, Name = "Macau", ArrivalDate = DateTime.Parse("Mar 21, 2015") },
            new Stop() { Order = 49, Latitude =  22.396428, Longitude =  114.109497, Name = "Hong Kong", ArrivalDate = DateTime.Parse("Mar 24, 2015") },
            new Stop() { Order = 50, Latitude =  39.904030, Longitude =  116.407526, Name = "Beijing, China", ArrivalDate = DateTime.Parse("Apr 19, 2015") },
            new Stop() { Order = 51, Latitude =  22.396428, Longitude =  114.109497, Name = "Hong Kong", ArrivalDate = DateTime.Parse("Apr 24, 2015") },
            new Stop() { Order = 52, Latitude =  1.352083, Longitude =  103.819836, Name = "Singapore", ArrivalDate = DateTime.Parse("Apr 30, 2015") },
            new Stop() { Order = 53, Latitude =  3.139003, Longitude =  101.686855, Name = "Kuala Lumpor, Malaysia", ArrivalDate = DateTime.Parse("May 7, 2015") },
            new Stop() { Order = 54, Latitude =  13.727896, Longitude =  100.524123, Name = "Bangkok, Thailand", ArrivalDate = DateTime.Parse("May 24, 2015") },
            new Stop() { Order = 55, Latitude =  33.748995, Longitude =  -84.387982, Name = "Atlanta, Georgia", ArrivalDate = DateTime.Parse("Jun 17, 2015") },

                    }
                };

                _context.Trips.Add(worldStop);
                _context.Stops.AddRange(worldStop.Stops);

                await _context.SaveChangesAsync();
            }
        }
    }
}
