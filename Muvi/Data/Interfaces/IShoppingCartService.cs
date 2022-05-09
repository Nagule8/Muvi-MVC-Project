using Muvi.Data.Cart;
using Muvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muvi.Data.Interfaces
{
    public interface IShoppingCartService
    {
        public ShoppingCart GetShoppingCart(IServiceProvider services);
        public void AddItemToCart(Movie movie);
        public void RemoveItemFromCart(Movie movie);
        public List<ShoppingCartItem> GetShoppingCartItems();
        public double GetShoppingCartTotal();
        public Task ClearShoppingCartAsync();
    }
}
