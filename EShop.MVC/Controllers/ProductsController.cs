using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EShop.MVC.Data;

namespace EShop.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;

        public ProductsController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var query = _db.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Name.Contains(search) ||
                    (p.Description ?? "").Contains(search));
            }

            var items = await query.OrderBy(p => p.Name).ToListAsync();

            // تمرير النص للـView عشان ينعرض في مربع البحث
            ViewData["CurrentSearch"] = search;

            return View(items);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }
    }
}
