using SQLite;

namespace MenuLogDemo.CartModel
{
    [Table("CartItem")]
    public class ItemCartModel
    {
        [AutoIncrement, PrimaryKey]
        public int CartItemId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int ItemQuantity { get; set; }
        public float ItemPrice { get; set; }
        public string ItemDough { get; set; }
        public string ItemSauce { get; set; }
    }
}
