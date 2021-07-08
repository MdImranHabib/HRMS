using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRMS.Data;
using HRMS.Models;

namespace HRMS.Controllers
{
    public class ResidentFlatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResidentFlatsController(ApplicationDbContext context)
        {
            _context = context;
        }
     
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ResidentFlats
                .Include(r => r.Flat)
                .Include(r => r.Resident);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var residentFlat = await _context.ResidentFlats
                .Include(r => r.Flat)
                .Include(r => r.Resident)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (residentFlat == null)
            {
                return NotFound();
            }

            return View(residentFlat);
        }

        #region Assign Flat

        public IActionResult Create()
        {         
            ViewBag.Flats = _context.Flats.Where(f => f.Status == false).ToList();
            ViewBag.Residents = _context.Residents.ToList();

            return View();
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ResidentId,FlatId,ArrivalDate,DepartureDate")] ResidentFlat residentFlat)
        {
            if (ModelState.IsValid)
            {   
                _context.Add(residentFlat);

                var flat = await _context.Flats.FindAsync(residentFlat.FlatId);
                flat.Status = true;
                _context.Update(flat);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Flats = _context.Flats.Where(f => f.Status == false).ToList();
            ViewBag.Residents = _context.Residents.ToList();
            return View(residentFlat);
        }

        #endregion

        #region Edit ResidentFlat

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var residentFlat = await _context.ResidentFlats.FindAsync(id);
            if (residentFlat == null)
            {
                return NotFound();
            }
            ViewData["FlatId"] = new SelectList(_context.Flats, "Id", "Name", residentFlat.FlatId);
            ViewData["ResidentId"] = new SelectList(_context.Residents, "Id", "Name", residentFlat.ResidentId);
            return View(residentFlat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ResidentId,FlatId,ArrivalDate,DepartureDate")] ResidentFlat residentFlat)
        {
            if (id != residentFlat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(residentFlat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResidentFlatExists(residentFlat.Id))
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
            ViewData["FlatId"] = new SelectList(_context.Flats, "Id", "Name", residentFlat.FlatId);
            ViewData["ResidentId"] = new SelectList(_context.Residents, "Id", "Name", residentFlat.ResidentId);
            return View(residentFlat);
        }

        #endregion

        #region Delete ResidentFlat

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var residentFlat = await _context.ResidentFlats
                .Include(r => r.Flat)
                .Include(r => r.Resident)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (residentFlat == null)
            {
                return NotFound();
            }

            return View(residentFlat);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var residentFlat = await _context.ResidentFlats.FindAsync(id);          

            var flat = await _context.Flats.FindAsync(residentFlat.FlatId);
            flat.Status = false;
            _context.Update(flat);

            _context.ResidentFlats.Remove(residentFlat);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion        

        private bool ResidentFlatExists(int id)
        {
            return _context.ResidentFlats.Any(e => e.Id == id);
        }
    }
}
