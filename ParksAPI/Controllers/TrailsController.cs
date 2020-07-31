using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ParksAPI.Models;
using ParksAPI.Models.DTOs;
using ParksAPI.Repository.IRepository;

namespace ParksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _trailRepo;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepo, IMapper mapper)
        {
            _trailRepo = trailRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of trails.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDTO>))]
        public IActionResult GetTrails()
        {
            var list = _trailRepo.GetTrails();
            var objDto = new List<TrailDTO>();
            foreach(var obj in list)
            {
                objDto.Add(_mapper.Map<TrailDTO>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual trail.
        /// </summary>
        /// <param name="id">The ID of trail</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int id)
        {
            var obj = _trailRepo.GetTrail(id);
            if (obj == null)
            {
                return NotFound();
            }
            var objDTO = _mapper.Map<TrailDTO>(obj);
            return Ok(objDTO);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailCreatelDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreatelDTO park)
        {
            if (park == null)
            {
                return BadRequest(ModelState);
            }

            if(_trailRepo.TrailExists(park.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }

            var parkObj = _mapper.Map<Trail>(park);
            if (!_trailRepo.CreateTrail(parkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {parkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail", new { id = parkObj.Id }, parkObj);
        }

        [HttpPatch("{id:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int id, [FromBody] TrailUpdatelDTO park)
        {
            if (park == null || park.Id != id)
            {
                return BadRequest(ModelState);
            }

            var parkObj = _mapper.Map<Trail>(park);
            if (!_trailRepo.UpdateTrail(parkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {parkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int id)
        {
            if (!_trailRepo.TrailExists(id))
            {
                return NotFound();
            }

            var parkObj = _trailRepo.GetTrail(id);
            if (!_trailRepo.DeleteTrail(parkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {parkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
