﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkWeb.Models;
using ParkWeb.Repository.IRepository;

namespace ParkWeb.Controllers
{
    public class NationalParksController : Controller
    {

        private readonly INationalParkRepository _npRepo;

        public NationalParksController(INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
        }

        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new { data = await _npRepo.GetAllAsync(SD.ParkAPI_URL) });
        }
    }
}
