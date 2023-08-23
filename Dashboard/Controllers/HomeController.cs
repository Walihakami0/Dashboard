using Dashboard.Data;
using Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DashDbContext _context;
        public HomeController(DashDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult CreateNewProduct(Product pr)
        {
            _context.Product.Add(pr);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult Index()
        {
            var pro = _context.Product.ToList();
            return View(pro);
        }

        public IActionResult ProductDetails()
        {
			var pro = _context.Product.ToList();
			return View(pro);
        }


		public IActionResult Nav()
        {
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
    }
}