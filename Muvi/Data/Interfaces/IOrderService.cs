using Muvi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muvi.Data.Interfaces
{
    public interface IOrderService
    {
        Task StoreOrderSync(List<ShoppingCartItem> items, string userId, string userEmailAddress);
        Task<List<Order>> GetOrderByUserIdAndRole(string userId, string userRole); 
    }
}
