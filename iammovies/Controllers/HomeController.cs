using Microsoft.AspNetCore.Mvc;
using iammovies.Models;

namespace iammovies.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context) => _context = context;

        // GET /
        public IActionResult Index()
        {
            return View(); // Mostra Views/Home/Index.cshtml
        }

        public IActionResult Privacy() => View();
        
        public IActionResult Landing()
        {
            return View();
        }
        public IActionResult Acerca()
        {
            return View();
        }
    
    }
}

