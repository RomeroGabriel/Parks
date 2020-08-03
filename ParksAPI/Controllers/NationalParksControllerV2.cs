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
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    //[Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParksAPIDocNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksControllerV2 : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly IMapper _mapper;

        public NationalParksControllerV2(INationalParkRepository npRepo, IMapper mapper)
        {
            _npRepo = npRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of national parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDTO>))]
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
    }
}