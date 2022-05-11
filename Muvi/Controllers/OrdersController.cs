using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Muvi.Data.Cart;
using Muvi.Data.Interfaces;
using Muvi.Data.ViewModels;
using Muvi.Models;
using Razorpay.Api;
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

        private readonly UserManager<ApplicationUser> _userManager;

        public const string _key = "rzp_test_g2ipH0Ua8Lu36d";
        public const string _secret = "fB1rv7rNpUP0pCOj9Ngk4IdK";

        public OrdersController(IMovieInterface movieservice, IOrderService orderService, ShoppingCart shoppingCart, UserManager<ApplicationUser> userManager)
        {
            _movieservice = movieservice;
            _shoppingCart = shoppingCart;
            _orderService = orderService;
            _userManager = userManager;
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

            var orderId = CreateOrder(response);

            RazorPayOptionsVM razorPayOptions = new RazorPayOptionsVM()
            {
                Key = _key,
                ShoppingCartTotal = response.ShoppingCartTotal,
                ShoppingCart = response.ShoppingCart,
                Currency = "INR",
                Name = "Muvi",
                Descripiton = "to understand payment gateway",
                ImageLogoUrl = "",
                OrderId = orderId,
                ProfileName = _userManager.GetUserName(User),
                Notes = new Dictionary<string, string>()
                {
                    { "note 1", "this is payment note" }, { "note 2", "here also, Payment notes" }
                }
            };

            return View(razorPayOptions);
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

            return View("OrderCompleted");
        }



        //for razor payment
        private string CreateOrder(ShoppingCartVM shoppingCartVM)
        {
            try
            {
                RazorpayClient client = new RazorpayClient(_key, _secret);

                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", shoppingCartVM.ShoppingCartTotal*100); // amount in the smallest currency unit
                options.Add("currency", "INR");

                Razorpay.Api.Order orderResponse = client.Order.Create(options);

                var orderId = orderResponse.Attributes["id"].ToString();

                return orderId;
            }

            catch (Exception)
            {
                return null;
            }
        }

    }
}
