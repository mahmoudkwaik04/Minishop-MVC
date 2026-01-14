
using System;
using System.Collections.Generic;

namespace EShop.MVC.Services
{
    public interface ICartService
    {
        IDictionary<Guid,int> GetCart();
        void Add(Guid productId, int quantity = 1);
        void Remove(Guid productId);
        void Clear();
        int Count();
    }
}
