using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HRMS.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using HRMS.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //return View();
            return Redirect("~/Identity/Account/Login");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();          
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalFlats = await _context.Flats.CountAsync();
            ViewBag.AllocatedFlats = await _context.Flats.Where(f => f.Status == true).CountAsync();
            ViewBag.TotalResidents = await _context.ResidentFlats.CountAsync();

            return View();
        }
    }
}
