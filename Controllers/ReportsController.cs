using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace HospitalManagementSystem.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var model = await _context.Report
                .Include(m => m.Patient)
                .Include(m => m.Service)
                .Include(m => m.ApplicationUser)
                .ToListAsync();

            return _context.Report != null ?
                        View(model) :
                        Problem("Entity set 'ApplicationDbContext.Report'  is null.");


        //var applicationDbContext = _context.Report
        //    .Include(r => r.ApplicationUser)
        //    .Include(r => r.Service)
        //    .Include(r => r.Patient);
        //return View(await applicationDbContext.ToListAsync());
    }

        // GET: Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
        if (id == null || _context.Report == null)
        {
            return NotFound();
        }

            var model = await _context.Report
                    .Include(m => m.Patient)
                    .Include(m => m.Service)
                    .ToListAsync();

            var patient = await _context.Patient
                    .FirstOrDefaultAsync(m => m.Id == id);

            var report = await _context.Report
            .FirstOrDefaultAsync(m => m.Id == id);
        if (report == null)
        {
            return NotFound();
        }

        return View(report);
    }


        [HttpGet]
        public async Task<IActionResult> GetPatientDetails(int patientId)
        {
            try
            {
                // Fetch the patient details from the database
                var patient = await _context.Patient
                    .Where(p => p.Id == patientId)
                    .Select(p => new
                    {
                        Name = p.Name,
                        dateofbirth = p.DateOfBirth,
                        Age = p.Age,
                        Address = p.Address,
                        Contact = p.ContactNumber
                    })
                    .FirstOrDefaultAsync();

                // Check if patient exists
                if (patient == null)
                {
                    return Json(new { success = false, message = "Patient not found." });
                }

                // Return the patient details in JSON format
                return Json(new { success = true, patient });
            }
            catch (Exception ex)
            {
                // Handle any exceptions (logging can be done here)
                return Json(new { success = false, message = "Error retrieving patient details." });
            }
        }




        [HttpGet]
        public async Task<IActionResult> GetPreviousReports(int patientId)
        {
            var previousReports = await _context.Report
                .Where(r => r.PatientId == patientId)
                .Include(r => r.Service)
                .ToListAsync();

            if (previousReports == null)
            {
                return Json(new { success = false, message = "No reports found for this patient." });
            }

            return Json(new { success = true, reports = previousReports });
        }



        // GET: Reports/Create
        public async Task<IActionResult> Create(int? patientId)
        {
            try
            {
                if (patientId.HasValue)
                {
                    // Fetch previous reports for the patient
                    var previousReports = _context.Report
                        .Where(r => r.PatientId == patientId)
                        .Include(r => r.Service)  // Ensure Service is included
                        .ToList();

                    //var patientDetails = _context.Patient
                    //    .Where(r => r.Id == patientId)
                    //    .FirstOrDefaultAsync();

                    ViewBag.PreviousReports = previousReports;
                    //ViewBag.GetPatientDetails = patientDetails;

                    // Fetch patient details
                    var patient = await _context.Patient.FindAsync(patientId);
                    ViewBag.PatientName = patient?.Name ?? "Unknown Patient";  // Handle null patient gracefully
                }

                // Prepare select lists for dropdowns
                ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name");
                ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Name", patientId);

                return View();
            }
            catch (Exception ex)
            {
                // Log the error and return a user-friendly error message
                // Logging could be added here for debugging purposes.
                TempData["ErrorMessage"] = "Error retrieving reports. Please try again later.";
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Reports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReportDate,SubmittedBy,ServiceId,PatientId,Summary")] Report report)
        {
            // Get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch the user's first and last name using UserManager
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                // Handle the case where the user is not found
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index", "Home");
            }

            // Concatenate first and last name
            report.SubmittedBy = $"{user.FirstName} {user.LastName}";
            report.ReportDate = DateTime.Now;

            // Save the report to the database
            _context.Add(report);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Report successfully submitted.";

            return RedirectToAction("Index", "Home");

            // If we reach here, something went wrong, re-populate the dropdowns
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name", report.ServiceId);
            ViewData["PatientId"] = new SelectList(_context.Patient, "Id", "Name", report.PatientId);

            return View(report);  // Return the view with the current report object
        }





        // GET: Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Report == null)
            {
                return NotFound();
            }

            var report = await _context.Report.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Name", report.ServiceId);
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BaranggayId,ReportDate,SubmittedBy,ServiceId,Summary")] Report report)
        {
            if (id != report.Id)
            {
                TempData["ErrorMessage"] = "Error Occured";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Successfully Updated";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceId"] = new SelectList(_context.Service, "Id", "Id", report.ServiceId);
            return View(report);
        }

        // GET: Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Report == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .Include(r => r.ApplicationUser)
                .Include(r => r.Service)
                .Include(r => r.Patient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Report == null)
            {
                TempData["ErrorMessage"] = "Error Occured";
                return Problem("Entity set 'ApplicationDbContext.MedicalHistory'  is null.");
            }
            var medicalHistory = await _context.Report.FindAsync(id);
            if (medicalHistory != null)
            {
                TempData["SuccessMessage"] = "Succesfully Deleted";
                _context.Report.Remove(medicalHistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
          return (_context.Report?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
