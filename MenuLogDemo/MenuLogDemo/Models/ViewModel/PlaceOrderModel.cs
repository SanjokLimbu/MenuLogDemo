using System;
using System.Collections.Generic;
using System.Text;

namespace MenuLogDemo.Models.ViewModel
{
    public class PlaceOrderModel
    {
        public string PlacedOrderItemname { get; set; }
        public int PlacedOrderQuantity { get; set; }
        public float PlacedOrderItemPrice { get; set; }
        public string PlacedOrderItemDough { get; set; }
        public string PlacedOrderItemSauce { get; set; }
    }
}
