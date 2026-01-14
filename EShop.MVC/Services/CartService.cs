
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Diagnostics;

namespace EShop.MVC.Services
{
    public class CartService : ICartService
    {
        private const string CartKey = "CART_ITEMS";
        private readonly IHttpContextAccessor _http;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CartService(IHttpContextAccessor http, IWebHostEnvironment webHostEnvironment)
        {
            _http = http;
            _webHostEnvironment = webHostEnvironment;
        }

        private void Log(string message)
        {
            Console.WriteLine($"[CART_LOG] {DateTime.Now}: {message}");
            Debug.WriteLine($"[CART_LOG] {DateTime.Now}: {message}");
            try
            {
                var logPath = Path.Combine(_webHostEnvironment.ContentRootPath, "cart_log.txt");
                System.IO.File.AppendAllText(logPath, $"{DateTime.Now}: {message}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CART_LOG_ERROR] {DateTime.Now}: Log file write error: {ex.Message}");
                Debug.WriteLine($"[CART_LOG_ERROR] {DateTime.Now}: Log file write error: {ex.Message}");
            }
        }

        public IDictionary<Guid,int> GetCart()
        {
            var session = _http.HttpContext!.Session;
            var json = session.GetString(CartKey);
            Log($"GetCart: Retrieved JSON from session: {json ?? "(null or empty)"}");
            if (string.IsNullOrEmpty(json)) return new Dictionary<Guid,int>();
            try
            {
                var cart = JsonSerializer.Deserialize<Dictionary<Guid,int>>(json);
                Log($"GetCart: Deserialized cart: {JsonSerializer.Serialize(cart)}");
                return cart ?? new Dictionary<Guid,int>();
            }
            catch (Exception ex)
            {
                Log($"GetCart: Deserialization error: {ex.Message}. JSON was: {json}");
                return new Dictionary<Guid,int>();
            }
        }

        private void Save(IDictionary<Guid,int> cart)
        {
            var session = _http.HttpContext!.Session;
            var json = JsonSerializer.Serialize(cart);
            session.SetString(CartKey, json);
            Log($"Save: Cart saved: {json}");
        }

        public void Add(Guid productId, int quantity = 1)
        {
            Log($"Add: Attempting to add product {productId} with quantity {quantity}");
            var cart = GetCart();
            cart[productId] = cart.ContainsKey(productId) ? cart[productId] + quantity : quantity;
            Log($"Add: Product {productId} added. Cart before save: {System.Text.Json.JsonSerializer.Serialize(cart)}");
            Save(cart);
            Log($"Add: Product {productId} added. Cart after save: {System.Text.Json.JsonSerializer.Serialize(cart)}");
        }

        public void Remove(Guid productId)
        {
            Log($"Remove: Attempting to remove product {productId}");
            var cart = GetCart();
            if (cart.ContainsKey(productId))
            {
                cart.Remove(productId);
                Log($"Remove: Product {productId} removed. Cart after remove: {JsonSerializer.Serialize(cart)}");
                Save(cart);
            }
            else
            {
                Log($"Remove: Product {productId} not found in cart.");
            }
        }

        public void Clear()
        {
            Log($"Clear: Attempting to clear cart.");
            Save(new Dictionary<Guid,int>());
            Log($"Clear: Cart cleared.");
        }

        public int Count()
        {
            int total = 0;
            foreach (var kv in GetCart()) total += kv.Value;
            Log($"Count: Current total items in cart: {total}");
            return total;
        }
    }
}


