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
    public class FlatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlatsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Flats.ToListAsync());
        }
    
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flat = await _context.Flats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flat == null)
            {
                return NotFound();
            }

            return View(flat);
        }

        #region Create Flat

        public IActionResult Create()
        {
            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Category,Description,Rent,MeterNo")] Flat flat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flat);
        }

        #endregion

        #region Edit Flat

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flat = await _context.Flats.FindAsync(id);
            if (flat == null)
            {
                return NotFound();
            }
            return View(flat);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Category,Description,Rent,MeterNo,Status")] Flat flat)
        {
            if (id != flat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlatExists(flat.Id))
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
            return View(flat);
        }

        #endregion

        #region Delete Flat

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flat = await _context.Flats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flat == null)
            {
                return NotFound();
            }

            return View(flat);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flat = await _context.Flats.FindAsync(id);
            _context.Flats.Remove(flat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        private bool FlatExists(int id)
        {
            return _context.Flats.Any(e => e.Id == id);
        }
    }
}
