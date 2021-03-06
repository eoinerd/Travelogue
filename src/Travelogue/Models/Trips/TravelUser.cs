﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Travelogue.Models
{
    public class TravelUser : IdentityUser
    {
        public DateTime FirstTrip { get; set; }

        public string Image { get; set; }
    }
}
