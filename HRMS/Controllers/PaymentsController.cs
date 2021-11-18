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
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }
     
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Payments.Include(p => p.Rent);
            return View(await applicationDbContext.ToListAsync());
        }
     
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Rent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }
      
        public IActionResult Create()
        {
            var rents = _context.Rents
                .Include(r => r.Flat)
                .Where(r => r.TotalBill > r.Paid).ToList();

            ViewBag.Rents = rents;
            ViewData["RentId"] = new SelectList(_context.Rents, "Id", "RentMonth");
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RentId,Amount,PaymentDate,Method")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                var rent = _context.Rents.FirstOrDefault(r => r.Id == payment.RentId);
                rent.Paid = rent.Paid + payment.Amount;

                _context.Add(payment);
                _context.Update(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var rents = _context.Rents
               .Include(r => r.Flat)
               .Where(r => r.TotalBill > r.Paid).ToList();

            ViewBag.Rents = rents;
            ViewData["RentId"] = new SelectList(_context.Rents, "Id", "RentMonth", payment.RentId);
            return View(payment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["RentId"] = new SelectList(_context.Rents, "Id", "RentMonth", payment.RentId);
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RentId,Amount,PaymentDate,Method")] Payment payment)
        {
            if (id != payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Id))
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
            ViewData["RentId"] = new SelectList(_context.Rents, "Id", "RentMonth", payment.RentId);
            return View(payment);
        }
 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Rent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }
   
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ReceivePayment(int rentId, double amount, string method)
        {
            Payment payment = new Payment()
            {
                RentId = rentId,
                Amount = amount,
                PaymentDate = DateTime.Now,
                Method = method
            };

            var rent = await _context.Rents.FirstOrDefaultAsync(r => r.Id == rentId);
            rent.Paid = rent.Paid + amount;

            _context.Add(payment);
            _context.Update(rent);
            await _context.SaveChangesAsync();

            var returnData = new
            {
                redirectUrl = Url.Action("Index", "Rents")
            };
            return Json(returnData);
        }
    }
}
