using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Controllers
{
    public class MorbiditiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MorbiditiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Morbidities
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Morbidity.Include(m => m.Report);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Morbidities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Morbidity == null)
            {
                return NotFound();
            }

            var morbidity = await _context.Morbidity
                .Include(m => m.Report)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (morbidity == null)
            {
                return NotFound();
            }

            return View(morbidity);
        }

        // GET: Morbidities/Create
        public IActionResult Create()
        {
            ViewData["ReportId"] = new SelectList(_context.Report, "Id", "Id");
            return View();
        }

        // POST: Morbidities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,ReportId,NotificationDate")] Morbidity morbidity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(morbidity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReportId"] = new SelectList(_context.Report, "Id", "Id", morbidity.ReportId);
            return View(morbidity);
        }

        // GET: Morbidities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Morbidity == null)
            {
                return NotFound();
            }

            var morbidity = await _context.Morbidity.FindAsync(id);
            if (morbidity == null)
            {
                return NotFound();
            }
            ViewData["ReportId"] = new SelectList(_context.Report, "Id", "Id", morbidity.ReportId);
            return View(morbidity);
        }

        // POST: Morbidities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status,ReportId,NotificationDate")] Morbidity morbidity)
        {
            if (id != morbidity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(morbidity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MorbidityExists(morbidity.Id))
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
            ViewData["ReportId"] = new SelectList(_context.Report, "Id", "Id", morbidity.ReportId);
            return View(morbidity);
        }

        // GET: Morbidities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Morbidity == null)
            {
                return NotFound();
            }

            var morbidity = await _context.Morbidity
                .Include(m => m.Report)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (morbidity == null)
            {
                return NotFound();
            }

            return View(morbidity);
        }

        // POST: Morbidities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Morbidity == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Morbidity'  is null.");
            }
            var morbidity = await _context.Morbidity.FindAsync(id);
            if (morbidity != null)
            {
                _context.Morbidity.Remove(morbidity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MorbidityExists(int id)
        {
          return (_context.Morbidity?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
