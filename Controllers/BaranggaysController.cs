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
    public class BaranggaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaranggaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Baranggays
        public async Task<IActionResult> Index()
        {
              return _context.Baranggay != null ? 
                          View(await _context.Baranggay.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Baranggay'  is null.");
        }

        // GET: Baranggays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Baranggay == null)
            {
                return NotFound();
            }

            var baranggay = await _context.Baranggay
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baranggay == null)
            {
                return NotFound();
            }

            return View(baranggay);
        }

        // GET: Baranggays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Baranggays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Baranggay baranggay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(baranggay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(baranggay);
        }

        // GET: Baranggays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Baranggay == null)
            {
                return NotFound();
            }

            var baranggay = await _context.Baranggay.FindAsync(id);
            if (baranggay == null)
            {
                return NotFound();
            }
            return View(baranggay);
        }

        // POST: Baranggays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Baranggay baranggay)
        {
            if (id != baranggay.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(baranggay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaranggayExists(baranggay.Id))
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
            return View(baranggay);
        }

        // GET: Baranggays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Baranggay == null)
            {
                return NotFound();
            }

            var baranggay = await _context.Baranggay
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baranggay == null)
            {
                return NotFound();
            }

            return View(baranggay);
        }

        // POST: Baranggays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Baranggay == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Baranggay'  is null.");
            }
            var baranggay = await _context.Baranggay.FindAsync(id);
            if (baranggay != null)
            {
                _context.Baranggay.Remove(baranggay);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BaranggayExists(int id)
        {
          return (_context.Baranggay?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
