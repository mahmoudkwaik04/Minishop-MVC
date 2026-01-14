using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EShop.MVC.Data;
using EShop.MVC.Models;
using EShop.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace EShop.MVC.Controllers.Admin
{
    [Route("Admin/Products")]
    [Controller]
    [Authorize(Roles = "Admin")]
    public class AdminProductsController : Controller
    {
        private readonly AppDbContext _context;

        public AdminProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
            return View(products);
        }

        // GET: Admin/Products/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    ImageUrl = model.ImageUrl,
                    Stock = model.Stock,
                    Category = model.Category,
                    IsAvailable = model.IsAvailable,
                    IsFeatured = model.IsFeatured,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Add(product);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "تم إضافة المنتج بنجاح!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/Products/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description ?? string.Empty,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock,
                Category = product.Category,
                IsAvailable = product.IsAvailable,
                IsFeatured = product.IsFeatured
            };

            return View(model);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.Products.FindAsync(id);
                    if (product == null)
                    {
                        return NotFound();
                    }

                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.Price = model.Price;
                    product.ImageUrl = model.ImageUrl;
                    product.Stock = model.Stock;
                    product.Category = model.Category;
                    product.IsAvailable = model.IsAvailable;
                    product.IsFeatured = model.IsFeatured;
                    product.UpdatedAt = DateTime.UtcNow;

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "تم تحديث المنتج بنجاح!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(model.Id))
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
            return View(model);
        }

        // GET: Admin/Products/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost("Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "تم حذف المنتج بنجاح!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}

