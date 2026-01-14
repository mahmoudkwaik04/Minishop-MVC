using Microsoft.AspNetCore.Mvc;
using EShop.MVC.Services;
using System.Threading.Tasks;

namespace EShop.MVC.ViewComponents
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly ICartService _cartService;

        public CartSummaryViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            int cartCount = _cartService.Count();
            return View(cartCount);
        }
    }
}

