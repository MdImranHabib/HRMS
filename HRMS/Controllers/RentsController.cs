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
    public class RentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        Dictionary<string, string> months = new Dictionary<string, string>();

        public RentsController(ApplicationDbContext context)
        {
            _context = context;

            var currentYear = DateTime.Now.Year;
            months.Add("January-" + currentYear, "January-" + currentYear);
            months.Add("February-" + currentYear, "February-" + currentYear);
            months.Add("March-" + currentYear, "March-" + currentYear);
            months.Add("April-" + currentYear, "April-" + currentYear);
            months.Add("May-" + currentYear, "May-" + currentYear);
            months.Add("June-" + currentYear, "June-" + currentYear);
            months.Add("July-" + currentYear, "July-" + currentYear);
            months.Add("August-" + currentYear, "August-" + currentYear);
            months.Add("September-" + currentYear, "September-" + currentYear);
            months.Add("October-" + currentYear, "October-" + currentYear);
            months.Add("November-" + currentYear, "November-" + currentYear);
            months.Add("December-" + currentYear, "December-" + currentYear);

            //months.Add("January", "January");
            //months.Add("February", "February");
            //months.Add("March", "March");
            //months.Add("April", "April");
            //months.Add("May", "May");
            //months.Add("June", "June");
            //months.Add("July", "July");
            //months.Add("August", "August");
            //months.Add("September", "September");
            //months.Add("October", "October");
            //months.Add("November", "November");
            //months.Add("December", "December");
        }
       
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rents.Include(r => r.Flat);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents
                .Include(r => r.Flat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        #region Rent Receive

        public IActionResult Create()
        {                                    
            ViewBag.Flats = _context.Flats.Where(f => f.Status == true).ToList();
            ViewBag.Months = months;
            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FlatId,RentMonth,FlatRent,ElectricBill,GasBill,WaterBill,TotalBill,Paid,Date,Remarks")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                if(rent.Paid > rent.TotalBill)
                {
                    rent.Paid = rent.TotalBill;
                }
                rent.Date = DateTime.Now;

                _context.Add(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Flats = _context.Flats.Where(f => f.Status == true).ToList();
            ViewBag.Months = months;
            return View(rent);
        }

        public JsonResult GetFlatInfoById(int id)
        {
            var flatInfo = _context.ResidentFlats
                .Include(rf => rf.Flat)
                .Include(rf => rf.Resident)
                .FirstOrDefault(rf => rf.FlatId == id);

            return Json(flatInfo);
        }

        public JsonResult GetRentInfoByFlatId(int id)
        {
            var rentInfo = _context.Rents.LastOrDefault(rf => rf.FlatId == id);

            return Json(rentInfo);
        }

        #endregion

        // GET: Rents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }
            ViewData["FlatId"] = new SelectList(_context.Flats, "Id", "Category", rent.FlatId);
            return View(rent);
        }

        // POST: Rents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FlatId,RentMonth,FlatRent,ElectricBill,GasBill,WaterBill,TotalBill,Paid,Date,Remarks")] Rent rent)
        {
            if (id != rent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentExists(rent.Id))
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
            ViewData["FlatId"] = new SelectList(_context.Flats, "Id", "Category", rent.FlatId);
            return View(rent);
        }

        // GET: Rents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents
                .Include(r => r.Flat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // POST: Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rent = await _context.Rents.FindAsync(id);
            _context.Rents.Remove(rent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentExists(int id)
        {
            return _context.Rents.Any(e => e.Id == id);
        }
    }
}
