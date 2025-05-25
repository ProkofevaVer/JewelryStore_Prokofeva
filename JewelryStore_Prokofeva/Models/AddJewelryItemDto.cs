namespace JewelryStore_Prokofeva.Models
{
    public class AddJewelryItemDto
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal PriceDiscounr { get; set; }
        public int Rating { get; set; }
        public string Articul { get; set; }
        public string PhotoUrl { get; set; }
        public int CategoryId { get; set; }
        public int MaterialId { get; set; }
        public int ForWhoId { get; set; }
        public int StockQuantity { get; set; }
        public decimal ApproximateWeight { get; set; }
    }
}
