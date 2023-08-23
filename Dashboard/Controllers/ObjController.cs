using Dashboard.Data;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class ObjController : Controller
    {
        private readonly DashboardContext _context;
        public ObjController(DashboardContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var a = _context.Users.ToList();
            ViewBag.User = a;

            return View();
        }
    }
}
