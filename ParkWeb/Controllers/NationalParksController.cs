using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkWeb.Models;
using ParkWeb.Repository.IRepository;

namespace ParkWeb.Controllers
{
    [Authorize]
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
            return Json(new { data = await _npRepo.GetAllAsync(SD.ParkAPI_URL, HttpContext.Session.GetString("Token")) });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark obj = new NationalPark();
            if (id == null)
            {
                return View(obj);
            }

            obj = await _npRepo.GetAsync(SD.ParkAPI_URL, id.GetValueOrDefault(), HttpContext.Session.GetString("Token"));
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark obj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using(var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }

                    obj.Picture = p1;
                }
                else
                {
                    var objDB = await _npRepo.GetAsync(SD.ParkAPI_URL, obj.Id, HttpContext.Session.GetString("Token"));
                    obj.Picture = objDB.Picture;
                }

                if (obj.Id == 0)
                {
                    await _npRepo.CreateAsync(SD.ParkAPI_URL, obj, HttpContext.Session.GetString("Token"));
                }
                else
                {
                    await _npRepo.UpdateAsync(SD.ParkAPI_URL + obj.Id, obj, HttpContext.Session.GetString("Token"));
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _npRepo.DeleteAsync(SD.ParkAPI_URL, id, HttpContext.Session.GetString("Token"));
            if (status)
            {
                return Json(new { success = true, message="Delete Successful" });
            }
            return Json(new { success = false, message = "Delete not Successful" });
        }
    }
}
