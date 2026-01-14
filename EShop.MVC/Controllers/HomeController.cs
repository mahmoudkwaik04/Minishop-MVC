using EShop.MVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShop.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _db.Products
                .Where(p => p.IsAvailable)
                .OrderByDescending(p => p.IsFeatured)
                .ThenByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(products);
        }
    }
}
