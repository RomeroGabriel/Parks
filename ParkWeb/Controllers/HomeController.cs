using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkWeb.Models;
using ParkWeb.Models.ViewModel;
using ParkWeb.Repository.IRepository;

namespace ParkWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;
        private readonly IAccountRepository _accountRepo;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository npRepo, ITrailRepository trailRepo, IAccountRepository accountRepo)
        {
            _logger = logger;
            _npRepo = npRepo;
            _trailRepo = trailRepo;
            _accountRepo = accountRepo;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM list = new IndexVM()
            {
                NationalParksList = await _npRepo.GetAllAsync(SD.ParkAPI_URL, HttpContext.Session.GetString("Token")),
                TrailsList = await _trailRepo.GetAllAsync(SD.TraiAPI_URL, HttpContext.Session.GetString("Token"))
            };
            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {
            User result = await _accountRepo.LoginAsync(SD.AccountAPI_URL + "authenticate/", obj);
            if (result.Token == null)
            {
                return View();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, result.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, result.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetString("Token", result.Token);
            TempData["alert"] = "Welcome " + result.Username;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            bool result = await _accountRepo.RegisterAsync(SD.AccountAPI_URL + "register/", obj);
            if (result == false)
            {
                return View();
            }
            TempData["alert"] = "Registeration Sucessful!";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("Token", "");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
