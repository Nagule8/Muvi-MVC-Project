using Muvi.Data.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Muvi.Data.ViewModels
{
    public class RazorPayOptionsVM
    {
        public string Key { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public string Descripiton { get; set; }
        public string ImageLogoUrl { get; set; }
        public string OrderId { get; set; }
        public string ProfileName { get; set; }
        public Dictionary<string, string> Notes  { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public double ShoppingCartTotal { get; set; }
    }
}
