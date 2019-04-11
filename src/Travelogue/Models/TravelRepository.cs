using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Data;
using Travelogue.ViewModels;

namespace Travelogue.Models
{
    public class TravelRepository : ITravelRepository
    {
        private StoryContext _context;

        public TravelRepository(StoryContext context)
        {
            _context = context;
        }

        public void AddStop(string tripName, Stop newStop, string username)
        {
            var trip = GetUserTripByName(tripName, username);

            if(trip != null)
            {
                trip.Stops.Add(newStop);
                _context.Stops.Add(newStop);
            }
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            return _context.Trips.ToList();
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                .Include(x => x.Stops)
                .Where(x => x.Name == tripName)
                .FirstOrDefault();
        }

        public IEnumerable<Trip> GetTripsByUsername(string name)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.UserName == name)
                .ToList();
        }

        public Trip GetUserTripByName(string tripName, string username)
        {
            return _context.Trips
                .Include(x => x.Stops)
                .Where(x => x.Name == tripName && x.UserName == username)
                .FirstOrDefault();
        }

        public async Task<bool> SaveChangesAsync()
        {
           return (await _context.SaveChangesAsync()) > 0;
        }

        public IList<Post> Posts(int pageNo, int pageSize)
        {
            var posts = _context.Posts.ToList();
                                  //.Where(p => p.Published)
                                  //.OrderByDescending(p => p.PostedOn)
                                  //.Skip(pageNo * pageSize)
                                  //.Take(pageSize)
                                  //.Fetch(p => p.Category)
                                  //.ToList();

            var postIds = posts.Select(p => p.Id).ToList();

            return _context.Posts
                  .Where(p => postIds.Contains(p.Id))
                  .OrderByDescending(p => p.PostedOn)
                  //.FetchMany(p => p.Tags)
                  .ToList();
        }

        public int TotalPosts()
        {
            return _context.Posts.Where(p => p.Published).Count();
        }

        public void UpdateStop(string trip, string stop, int postIdForStopUpdate)
        {
            var resultStop = (from t in _context.Trips
                             join s in _context.Stops 
                             on t.Id equals s.TripId
                             where stop == s.Name
                             select s).FirstOrDefault();

            resultStop.PostId = postIdForStopUpdate;

            _context.Stops.Update(resultStop);
        }
    }
}
