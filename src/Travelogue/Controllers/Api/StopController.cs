using AutoMapper;
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
    [Route("/api/stop/{tripName}/{stopName}")]
    public class StopController : Controller
    {
        private ITravelRepository _repository;
        private ILogger<StopsController> _logger;
        private GeoCoordsService _coords;

        public StopController(ITravelRepository repository, ILogger<StopsController> logger,
            GeoCoordsService coords)
        {
            _repository = repository;
            _logger = logger;
            _coords = coords;
        }

        [HttpGet("")]
        public IActionResult Get(string stopName, string tripName)
        {
            try
            {
                var trip = _repository.GetUserTripByName(tripName, this.User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<StopsViewModel>>(trip.Stops.Where(x => x.Name == stopName).ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to get stops: {0}", ex);
            }

            return BadRequest("Failed to get stops");
        }
    }
}
