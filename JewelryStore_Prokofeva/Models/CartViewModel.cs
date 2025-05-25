namespace JewelryStore_Prokofeva.Models
{
    public class CartViewModel
    {
        public IEnumerable<dynamic> CartItems { get; set; }
        public decimal TotalOriginalPrice { get; set; }
        public decimal TotalDiscountPrice { get; set; }
        public decimal TotalDiscount { get; set; }
    }
}
