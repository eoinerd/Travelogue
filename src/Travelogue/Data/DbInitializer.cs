using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;

namespace Travelogue.Data
{
    public class DbInitializer
    {
        public static void Initialize(StoryContext context, UserManager<TravelUser> _userManager)
        {
            context.Database.EnsureCreated();

            if ( _userManager.FindByEmailAsync("eoinerd@gmail.com") == null)
            {
                var user = new TravelUser()
                {
                    UserName = "eoiner",
                    Email = "eoinerd@gmail.com"
                };

                _userManager.CreateAsync(user, "Fiatpunto26%20");
            }

            //// Look for any students.
            //if (context.Posts.Any())
            //{
            //    return;   // DB has been seeded
            //}

            //var posts = new Post[]
            //{
            //    new Post{Title="Siem Reap", Text="blah blah blah", BlogId=1, PostedOn=DateTime.Now, Published=true, UserId=1},
            //    new Post{Title="Koh Rong", Text="boob obobobob", BlogId=1, PostedOn=DateTime.Now, Published=true, UserId=1},
            //    new Post{Title="Phnom Penh", Text="asdasdfasdasf asdasd", BlogId=1, PostedOn=DateTime.Now, Published=true, UserId=1}
            //};
            //foreach (Post p in posts)
            //{
            //    context.Posts.Add(p);
            //}

            //context.SaveChanges();

            if (!context.Trips.Any())
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

                context.Trips.Add(usTrip);
                context.Stops.AddRange(usTrip.Stops);

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
            new Stop() { Order = 21, Latitude =  53.349805, Longitude =  -6.260310, Name = "Dublin, Ireland", ArrivalDate = DateTime.Parse("Sep 2, 2014") },
            new Stop() { Order = 22, Latitude =  54.597285, Longitude =  -5.930120, Name = "Belfast, Northern Ireland", ArrivalDate = DateTime.Parse("Sep 7, 2014") },
            new Stop() { Order = 23, Latitude =  53.349805, Longitude =  -6.260310, Name = "Dublin, Ireland", ArrivalDate = DateTime.Parse("Sep 9, 2014") },
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


                context.Trips.Add(worldStop);
                context.Stops.AddRange(worldStop.Stops);

                context.SaveChanges();
            }
        }
    }
}
