using Dashboard.Data;
using Dashboard.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model.Map;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using MimeKit;


namespace Dashboard.Controllers.Shop
{
    public class ShopController : Controller
    {
        private readonly DashDbContext _context;

        private readonly DashboardContext _context2;


        public ShopController(DashDbContext context, DashboardContext context2)
        {
            _context = context;
            _context2 = context2;


        }

        
        public IActionResult Index()
        {
			var product = _context.Product.ToList();

			var productd = _context.ProductDetails.ToList();
            ViewBag.prod = productd;
			return View(product);
        }

        public IActionResult Detailsp(int id)
        {
            var prodone = _context.ProductDetails.SingleOrDefault(p => p.Id == id);
            var prod = _context.Product.SingleOrDefault(p => p.Id == prodone.ProductId);

            ViewBag.prod = prod;
            return View(prodone);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Cart(int id)
        {
            var user = HttpContext.User.Identity.Name;

            if (id != null)
            {
                var prodone = _context.ProductDetails.SingleOrDefault(p => p.Id == id);
                var prod = _context.Product.SingleOrDefault(p => p.Id == prodone.ProductId);
                double a = Decimal.ToDouble(prodone.Price);

                var cart = new Cart()
                {
                    EmailC = user,
                    MyProductId = prodone.ProductId,
                    ProductName = prod.ProductName,
                    Price = a,
                    Image = prodone.Image,
                    Tax = 0.15
                };

                _context.Add(cart);
                _context.SaveChanges();
            }
            

            

            var ProductCart = _context.Cart.ToList();
            int X = 0;
            double total = 0;

            foreach (var p in ProductCart)
            {
                ++X;
                total += p.Price;

            }
            ViewBag.user = user;

            ViewBag.total = total;
            ViewBag.conut = X;

            return View(ProductCart);
        }

        [Authorize]
        public IActionResult Cart()
        {
            var ProductCart = _context.Cart.ToList();
            int X = 0;
            double total = 0;
             foreach(var p in ProductCart)
            {
                ++X;
                total += p.Price;

            }
            var user = HttpContext.User.Identity.Name;
            ViewBag.user = user;

            ViewBag.total = total;
            ViewBag.conut = X;
            return View(ProductCart);


        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var cart = _context.Cart.SingleOrDefault(a => a.Id == id);
            _context.Cart.Remove(cart);
            _context.SaveChanges();
            return RedirectToAction("Cart");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Invoice(string email)
        {
            var cart = _context.Cart.Where(a => a.EmailC == email).ToList();
           // string date = DateTime.Now.ToString("MM/dd/yyyy");

            DateTime currentDateTime = DateTime.Now;
            foreach (var item in cart)
            {
                Payments payments = new Payments()
                {
                    EmailC = email,
                    MyProductId = item.MyProductId,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Image = item.Image,
                    Tax = 0.15,
                    DT = currentDateTime
                };
                _context.Add<Payments>(payments);
                _context.Cart.Remove(item);
                _context.SaveChanges();
             

            }
            SendMail(email);
            var usr = _context2.Users.SingleOrDefault(a => a.Email == email);
            ViewBag.username = usr.FirstName + " " + usr.LastName;

            var paym = _context.Payments.Where(a => a.EmailC == email).ToList();
            foreach(var dt in paym)
            {
                ViewBag.datetime = dt.DT;
				_context.Payments.Remove(dt);
				_context.SaveChanges();

			}
            ViewBag.Paym = paym;
			return View();
        }

        [Authorize]
        public IActionResult Invoice()
        {
            var email = HttpContext.User.Identity.Name;
            var usr = _context2.Users.SingleOrDefault(a => a.Email == email);
            ViewBag.username = usr.FirstName + " " + usr.LastName;
            var paym = _context.Payments.Where(a => a.EmailC == email).ToList();
            foreach (var dt in paym)
            {
                ViewBag.datetime = dt.DT;
				_context.Payments.Remove(dt);
                _context.SaveChanges();
			}
			ViewBag.Paym = paym;

			return View();
        }

        [Authorize]
        public IActionResult de(int id)
        {

            var dee = _context.Payments.SingleOrDefault(a=>a.Id == id);
            _context.Payments.Remove(dee);
            _context.SaveChanges();

            return RedirectToAction("Invoice");
        }

        public async Task<string> SendMail(string email)
        {
            //xxoiqetkzpzyuxzc
            //nzufhzxvbmvqyves

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("فاتورة الشراء", "appkits2030@gmail.com"));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "موقع تسوق";
            message.Body = new TextPart("plain")
            {
                Text = "شكرا على التسوق من متجرنا"
            };
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587);
                    client.Authenticate("appkits2030@gmail.com", "yinixnznjbvtgila");
                    await client.SendAsync(message);
                    client.Disconnect(true);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }



            };

            return "ok";

        }

    }
}
