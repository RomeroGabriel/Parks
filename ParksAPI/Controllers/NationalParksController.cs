using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ParksAPI.Models;
using ParksAPI.Repository.IRepository;

namespace ParksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : Controller
    {
        private INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var list = _npRepo.GetNationalParks();
            var objDto = new List<NationalParkDTO>();
            foreach(var obj in list)
            {
                objDto.Add(_mapper.Map<NationalParkDTO>(obj));
            }

            return Ok(objDto);
        }

        [HttpGet("{id:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int id)
        {
            var obj = _npRepo.GetNational(id);
            if (obj == null)
            {
                return NotFound();
            }
            var objDTO = _mapper.Map<NationalParkDTO>(obj);
            return Ok(objDTO);
        }

        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDTO park)
        {
            if (park == null)
            {
                return BadRequest(ModelState);
            }

            if(_npRepo.NationalParkExists(park.Name))
            {
                ModelState.AddModelError("", "National Park Exists!");
                return StatusCode(404, ModelState);
            }

            var parkObj = _mapper.Map<NationalPark>(park);
            if (!_npRepo.CreateNationalPark(parkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {parkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { id = parkObj.Id }, parkObj);
        }

        [HttpPatch("{id:int}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int id, [FromBody] NationalParkDTO park)
        {
            if (park == null || park.Id != id)
            {
                return BadRequest(ModelState);
            }

            var parkObj = _mapper.Map<NationalPark>(park);
            if (!_npRepo.UpdateNationalPark(parkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {parkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "DeleteNationalPark")]
        public IActionResult DeleteNationalPark(int id)
        {
            if (!_npRepo.NationalParkExists(id))
            {
                return NotFound();
            }

            var parkObj = _npRepo.GetNational(id);
            if (!_npRepo.DeleteNationalPark(parkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {parkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
