using System;
using System.Collections.Generic;
using System.Text;

namespace MenuLogDemo.Models.ViewModel
{
    public class Order
    {
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public float TotalCost { get; set; }
        public List<PlaceOrderModel> OrderItems { get; set; }
    }
}
