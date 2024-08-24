using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace HospitalManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identities.Any())
            {
                return Redirect("/identity/account/login");
            }

            if (TempData.ContainsKey("ErrorPassword"))
            {
                var error = TempData["ErrorPassword"];
                ViewBag.Message = new { Text = error, IsError = true };
            }
            else if (TempData.ContainsKey("SuccessMessage"))
            {
                var success = TempData["SuccessMessage"];
                ViewBag.Message = new { Text = success, IsError = false };
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);

            string? firstName = user?.FirstName;
            string? lastName = user?.LastName;
            string? email = user?.Email;

            ViewBag.Fullname = firstName + " " + lastName;
            ViewBag.Email = email;


            //recentHistory = await _context.MedicalHistory
            //    .Include(m => m.PatientProfile)
            //    .ToListAsync();

            //return 

            //var _userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return View();

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
    }
}
