using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace HospitalManagementSystem.Controllers
{
    [Authorize]
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
                return Redirect("/Identity/Account/Login");
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

            var childService = await _context.Report
                .Where(r => r.ServiceId.ToString().Contains("1"))
                .CountAsync();
            var maternalService = await _context.Report
                .Where(r => r.ServiceId.ToString().Contains("2"))
                .CountAsync();
            var familyService = await _context.Report
                .Where(r => r.ServiceId.ToString().Contains("3"))
                .CountAsync();
            var morbidityService = await _context.Report
                .Where(r => r.ServiceId.ToString().Contains("4"))
                .CountAsync();

            //float childAverage = (childService / 1234) * 100;
            //float maternalAverage = (maternalService / 1234) * 100;
            //float familyplanningAverage = (familyService / 1234) * 100;
            //float mobidityAverage = (morbidityService / 1234) * 100;

            float totalService = childService + morbidityService + maternalService + familyService;
            int totalPopulation = 78;
            float totalAvg = (totalService / totalPopulation) * 100;

            // Limiting decimal places to 3
            string formattedTotalAvg = totalAvg.ToString("0.##");

            ViewBag.Total = formattedTotalAvg;

            ViewBag.ServiceGraphData = new float[] { childService, maternalService, familyService, morbidityService };
            ViewBag.ServiceLabels = new string[] { "Child Care", "Maternal Care", "Family Planning", "Morbidity" };

            var reportCount = await _context.Report.CountAsync();
            var patientCount = await _context.Patient.CountAsync();

            ViewBag.transactionCount = reportCount;
            ViewBag.patientCount = patientCount;


            var user = await _userManager.GetUserAsync(HttpContext.User);

            string? firstName = user?.FirstName;
            string? lastName = user?.LastName;
            string? email = user?.UserName;

            ViewBag.Fullname = firstName + " " + lastName;
            ViewBag.Email = email;


            //recentHistory = await _context.MedicalHistory
            //    .Include(m => m.PatientProfile)
            //    .ToListAsync();

            //return 

            var _userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return _context.Report != null ?
                       View(await _context.Report
                       .Include(m => m.Patient)
                       //.Where(m => m.SubmittedBy == _userId)
                       .Include(m => m.ApplicationUser)
                       .Include(m => m.Service)
                       .ToListAsync())
                       : Problem("Entity set 'ApplicationDbContext.Report' is null.");

            //return View();

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
