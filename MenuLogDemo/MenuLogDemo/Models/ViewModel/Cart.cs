using MenuLogDemo.CartModel;
using MenuLogDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace MenuLogDemo.Models.ViewModel
{
    public class Cart
    {
        public string UserName { get; set; }
        public int UserNumber { get; set; }
        public string UserItemname { get; set; }
        public int UserItemQuantity { get; set; }
        public float userItemPrice { get; set; }
        public string ItemDough { get; set; }
        public string ItemSauce { get; set; }
    }
}
