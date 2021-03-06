﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Travelogue.Models.Blogs;
using Travelogue.ViewModels;

namespace Travelogue.Models
{
    public interface ITravelRepository
    {
        IEnumerable<Trip> GetAllTrips();

        void AddTrip(Trip trip);

        Task<bool> SaveChangesAsync();

        Trip GetTripByName(string tripName);

        void AddStop(string tripName, Stop newStop, string username);

        IEnumerable<Trip> GetTripsByUsername(string username);

        Trip GetUserTripByName(string tripName, string username);

        IList<Post> Posts(int pageNo, int pageSize);

        int TotalPosts();

        void UpdateStop(string trip, string stop, int postIdForStopUpdate);
    }
}