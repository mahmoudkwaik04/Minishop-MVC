using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EShop.MVC.Data;
using EShop.MVC.Services;
using EShop.MVC.ViewModels;
using System.Collections.Generic; // Dictionary için gerekli

namespace EShop.MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ICartService _cart;

        public CartController(AppDbContext db, ICartService cart)
        {
            _db = db;
            _cart = cart;
        }

        public async Task<IActionResult> Index()
        {
            var bag = _cart.GetCart();
            var ids = bag.Keys.ToList();
            
            // Eğer sepet boşsa, gereksiz SQL sorgusu çalıştırmamak için erken çıkış yap
            if (!ids.Any())
            {
                ViewBag.Total = 0;
                return View(new List<CartItemViewModel>()); // Boş bir liste döndür
            }

            var products = await _db.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
            var model = products.Select(p => new CartItemViewModel { Product = p, Quantity = bag[p.Id] }).ToList();
            ViewBag.Total = model.Sum(i => i.Product.Price * i.Quantity);
            return View(model);
        }

        // Mevcut Add metodu: Sepete ekler ve Index sayfasına yönlendirir (geleneksel form gönderimleri için)
        [HttpPost]
        public IActionResult Add(Guid id, int quantity = 1)
        {
            _cart.Add(id, quantity);
            return RedirectToAction("Index");
        }

        // Yeni AddToCartAjax metodu: Sepete ekler ve JSON yanıtı döndürür (AJAX istekleri için)
        [HttpPost]
       public IActionResult AddToCartAjax(Guid productId, int quantity = 1)
{
    try
    {
        _cart.Add(productId, quantity);
        var newCartCount = GetCartItemCount(); // جلب عدد المنتجات الجديد في السلة

        return Json(new 
        { 
            success = true, 
            newCartCount = newCartCount, 
            message = "تمت إضافة المنتج إلى السلة بنجاح." 
        });
    }
    catch (Exception ex)
    {
        // في حالة حدوث خطأ نعيد رسالة خطأ للمستخدم
        // يمكن أيضًا عمل Log للخطأ عبر _logger.LogError(ex, "خطأ أثناء إضافة المنتج للسلة");
        return Json(new 
        { 
            success = false, 
            message = "حدث خطأ أثناء إضافة المنتج إلى السلة: " + ex.Message 
        });
    }
}


        // Mevcut Remove metodu: Sepetten kaldırır ve Index sayfasına yönlendirir
        [HttpPost]
        public IActionResult Remove(Guid id)
        {
            _cart.Remove(id);
            return RedirectToAction("Index");
        }

        // Yeni RemoveAjax metodu: Sepetten kaldırır ve JSON yanıtı döndürür (AJAX istekleri için)
        [HttpPost]
        public IActionResult RemoveAjax(Guid id)
        {
            try
            {
                _cart.Remove(id);
                var newCartCount = GetCartItemCount();
                return Json(new { success = true, newCartCount = newCartCount, message = "Ürün sepetten başarıyla kaldırıldı." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ürün sepetten kaldırılırken bir hata oluştu: " + ex.Message });
            }
        }

        // Mevcut Clear metodu: Sepeti temizler ve Index sayfasına yönlendirir
        [HttpPost]
        public IActionResult Clear()
        {
            _cart.Clear();
            return RedirectToAction("Index");
        }

        // Yeni ClearAjax metodu: Sepeti temizler ve JSON yanıtı döndürür (AJAX istekleri için)
        [HttpPost]
        public IActionResult ClearAjax()
        {
            try
            {
                _cart.Clear();
                return Json(new { success = true, newCartCount = 0, message = "Sepet başarıyla temizlendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Sepet temizlenirken bir hata oluştu: " + ex.Message });
            }
        }

        public IActionResult Checkout()
        {
            // التحقق من تسجيل الدخول قبل إتمام الشراء
            if (!User.Identity.IsAuthenticated)
            {
                // توجيه المستخدم لصفحة تسجيل الدخول
                return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Checkout", "Cart") });
            }
            
            return View();
        }


        private int GetCartItemCount()
        {
            var cartItems = _cart.GetCart(); 
            return cartItems.Sum(item => item.Value); 
        }
    }
}