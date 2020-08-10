using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkWeb.Models;
using ParkWeb.Models.ViewModel;
using ParkWeb.Repository.IRepository;

namespace ParkWeb.Controllers
{
    [Authorize]
    public class TrailsController : Controller
    {

        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;

        public TrailsController(INationalParkRepository npRepo, ITrailRepository trailRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
        }

        public IActionResult Index()
        {
            return View(new TrailsVM() { });
        }

        public async Task<IActionResult> GetAllTrails()
        {
            return Json(new { data = await _trailRepo.GetAllAsync(SD.TraiAPI_URL, HttpContext.Session.GetString("Token")) });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.ParkAPI_URL, HttpContext.Session.GetString("Token"));

            TrailsVM objVM = new TrailsVM()
            {
                NationalParkList = npList.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trail = new Trail()
            };

            if (id == null)
            {
                return View(objVM);
            }

            objVM.Trail = await _trailRepo.GetAsync(SD.TraiAPI_URL, id.GetValueOrDefault(), HttpContext.Session.GetString("Token"));
            if (objVM.Trail == null)
            {
                return NotFound();
            }

            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Trail.Id == 0)
                {
                    await _trailRepo.CreateAsync(SD.TraiAPI_URL, obj.Trail, HttpContext.Session.GetString("Token"));
                }
                else
                {
                    await _trailRepo.UpdateAsync(SD.TraiAPI_URL + obj.Trail.Id, obj.Trail, HttpContext.Session.GetString("Token"));
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.ParkAPI_URL, HttpContext.Session.GetString("Token"));

                TrailsVM objVM = new TrailsVM()
                {
                    NationalParkList = npList.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = obj.Trail
                };
                return View(objVM);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(SD.TraiAPI_URL, id, HttpContext.Session.GetString("Token"));
            if (status)
            {
                return Json(new { success = true, message="Delete Successful" });
            }
            return Json(new { success = false, message = "Delete not Successful" });
        }
    }
}
