using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Hosting;
using AspNetCore.Reporting;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.Controllers
{
    [Authorize]
    public class RentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        Dictionary<string, string> months = new Dictionary<string, string>();

        public RentsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;

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

            var residentInfo = _context.ResidentFlats            
                .Include(rf => rf.Resident)
                .FirstOrDefault(rf => rf.FlatId == rent.FlatId);

            ViewBag.ResidentInfo = residentInfo;

            return View(rent);
        }

        #region Generate Rent

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

        public JsonResult IsMonthExist(int id, string month)
        {
            var rentInfo = _context.Rents.Any(rf => rf.FlatId == id && rf.RentMonth == month);           
            
            return Json(rentInfo);
        }

        #endregion

        #region Edit Rent

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
            ViewData["FlatId"] = new SelectList(_context.Flats, "Id", "Name", rent.FlatId);            
            return View(rent);
        }
   
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

        #endregion

        #region Delete Rent

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

            var residentInfo = _context.ResidentFlats
                .Include(rf => rf.Resident)
                .FirstOrDefault(rf => rf.FlatId == rent.FlatId);

            ViewBag.ResidentInfo = residentInfo;

            return View(rent);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rent = await _context.Rents.FindAsync(id);
            _context.Rents.Remove(rent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        private bool RentExists(int id)
        {
            return _context.Rents.Any(e => e.Id == id);
        }

        #region Rent Receipt

        public IActionResult Receipt(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = _context.Rents
                .Include(r => r.Flat)
                .FirstOrDefault(m => m.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            var resident = _context.ResidentFlats
                .Include(rf => rf.Resident)
                .FirstOrDefault(rf => rf.FlatId == rent.FlatId);

            string mimtype = "";
            int extension = 1;
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "Reports", "RentReceipt.rdlc");
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("FlatName", rent.Flat.Name);
            parameters.Add("FlatCategory", rent.Flat.Category);
            parameters.Add("MeterNo", rent.Flat.MeterNo);
            parameters.Add("ResidentName", resident.Resident.Name);
            parameters.Add("ResidentContact", resident.Resident.ContactNo);
            parameters.Add("ResidentNID", resident.Resident.NIDNo);
            parameters.Add("RentMonth", rent.RentMonth);
            parameters.Add("BillingDate", rent.Date.ToString("dd-MMM-yyyy hh:mm tt"));
            parameters.Add("FlatRent", rent.FlatRent.ToString());
            parameters.Add("ElectricBill", rent.ElectricBill.ToString());
            parameters.Add("GasBill", rent.GasBill.ToString());
            parameters.Add("WaterBill", rent.WaterBill.ToString());
            parameters.Add("TotalBill", rent.TotalBill.ToString());
            parameters.Add("Paid", rent.Paid.ToString());
            parameters.Add("Due", (rent.TotalBill - rent.Paid).ToString());
            parameters.Add("Remarks", rent.Remarks);

            LocalReport localReport = new LocalReport(path);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);
            return File(result.MainStream, "application/pdf");
        }

        #endregion
    }
}
