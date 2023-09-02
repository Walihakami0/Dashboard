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
		[Authorize]
        [HttpPost]
        public IActionResult CreateNewProduct(Product product)
        {
            _context.Product.Add(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [Authorize]
        public IActionResult Index()
        {
			var email = HttpContext.User.Identity.Name;
			CookieOptions options = new CookieOptions();
			options.Expires = DateTime.Now.AddMinutes(2);
			Response.Cookies.Append("Name", email, options);

			ViewBag.E = email;
            var pro = _context.Product.ToList();
            return View(pro);
        }

		[Authorize]
		public IActionResult ProductDetails()
		{
			var pro = _context.Product.ToList();
			ViewBag.Prod = _context.ProductDetails.ToList();

			ViewBag.email = Request.Cookies["Name"];
			ViewBag.product = pro;
			return View();
		}

		[Authorize]
		[HttpPost]
		public IActionResult ProductDetails(int Id)
		{
			var Product = _context.ProductDetails.Where(x => x.ProductId == Id).ToList();
			ViewBag.Prod = Product;
			var pro = _context.Product.ToList();
			ViewBag.product = pro;

			return View();
		}

		[Authorize]
		[HttpPost]
		public IActionResult search(string Name)
		{
			var product = _context.Product.Where(x => x.ProductName.Contains(Name)).ToList();
			ViewBag.product = product;
			var pro = _context.ProductDetails.ToList();
            ViewBag.Prod = pro;
			return RedirectToAction("ProductDetails");
		}

		[Authorize]
		[HttpPost]
        public IActionResult CreateDetalis(ProductDetails productDetails)
        {
			_context.ProductDetails.Add(productDetails);
			_context.SaveChanges();

			return RedirectToAction("ProductDetails");
        }

		[Authorize]
		public IActionResult Delete(int id)
		{
			var p = _context.ProductDetails.SingleOrDefault(p => p.Id == id);
			if (p != null)
			{
				_context.ProductDetails.Remove(p);
				_context.SaveChanges();
			}
			return RedirectToAction("ProductDetails");
		}

		[Authorize]
		public IActionResult DeleteP(int id)
		{
			var p = _context.Product.SingleOrDefault(p => p.Id == id);
			if (p != null)
			{
				_context.Product.Remove(p);
				_context.SaveChanges();
			}
			return RedirectToAction("Index");
		}

		[Authorize]
		public IActionResult Edit(int id)
		{
			var Productd = _context.ProductDetails.SingleOrDefault(p => p.Id == id);
			ViewBag.P = Productd;
			return View(Productd);
		}

		[Authorize]
		[HttpPost]
		public IActionResult Edit(ProductDetails prod)
		{
			ProductDetails p = _context.ProductDetails.SingleOrDefault(x => x.Id == prod.Id) ?? new ProductDetails();
			p.Description = prod.Description;
			p.Price = prod.Price;
			p.Model = prod.Model;
			p.Color = prod.Color;
			p.Image = prod.Image;
			_context.SaveChanges();
			return RedirectToAction("ProductDetails");
		}

		[Authorize]
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