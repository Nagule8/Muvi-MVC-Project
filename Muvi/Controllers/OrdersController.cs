using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Muvi.Data.Cart;
using Muvi.Data.Interfaces;
using Muvi.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Muvi.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IMovieInterface _movieservice;
        private readonly IOrderService _orderService;
        private readonly ShoppingCart _shoppingCart;

        public OrdersController(IMovieInterface movieservice, IOrderService orderService, ShoppingCart shoppingCart)
        {
            _movieservice = movieservice;
            _shoppingCart = shoppingCart;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await _orderService.GetOrderByUserIdAndRole(userId, userRole);

            return View(orders);
        }

        public IActionResult ShoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCatrTotal()
            };

            return View(response);
        }

        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            var item = await _movieservice.GetMovieById(id);

            if (item != null)
            {
                _shoppingCart.AddItemToCart (item);
            }

            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await _movieservice.GetMovieById(id);

            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }

            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            await _orderService.StoreOrderSync(items, userId, userEmail);
            await _shoppingCart.ClearShoppingCart();

            return View("Order Completed.");
        }

    }
}
