
using EShop.MVC.Models;

namespace EShop.MVC.ViewModels
{
    public class CartItemViewModel
    {
        public Product Product { get; set; } = new Product();
        public int Quantity { get; set; }
    }
}
