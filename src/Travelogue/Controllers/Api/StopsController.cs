﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;
using Travelogue.Services;
using Travelogue.ViewModels;

namespace Travelogue.Controllers.Api
{
    [Authorize]
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        private ITravelRepository _repository;
        private ILogger<StopsController> _logger;
        private GeoCoordsService _coords;

        public StopsController(ITravelRepository repository, ILogger<StopsController> logger,
            GeoCoordsService coords)
        {
            _repository = repository;
            _logger = logger;
            _coords = coords;
        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                // Get and return the stops for a trip by its name
                var trip = _repository.GetUserTripByName(tripName, this.User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<StopsViewModel>>(trip.Stops.OrderBy(x => x.ArrivalDate).ToList()));
            }
            catch(Exception ex)
            {
                _logger.LogError("Failed to get stops: {0}", ex);
            }

            return BadRequest("Failed to get stops");
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody]StopsViewModel vm)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(vm);

                    // get a stops coordinates
                    var result = await _coords.GetCoordsAsync(newStop.Name);

                    if(!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;

                        // Add new stop to the DB
                        _repository.AddStop(tripName, newStop, User.Identity.Name);

                        if(await _repository.SaveChangesAsync())
                        {
                            return Created($"/api/trips/{tripName}/stops/{newStop.Name}",
                            Mapper.Map<StopsViewModel>(newStop));
                        } 
                    }
                    
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Failed to save new stop:{0}", ex.Message);
            }

            return BadRequest("Failed to save new stop");
        }
    }
}
