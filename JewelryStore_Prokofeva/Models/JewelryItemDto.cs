namespace Api_JewelryStore.Models.DTO_Models
{
    public class JewelryItemDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public decimal? Price { get; set; }
        public int? Discount { get; set; }
        public decimal? PriceDiscounr { get; set; }
        public float? Rating { get; set; }
        public string? Articul { get; set; }
        public string? PhotoUrl { get; set; }
        public int? StockQuantity { get; set; }
        public float? ApproximateWeight { get; set; }
        public virtual CategoryDto? Category { get; set; }
        public MaterialDto? Material { get; set; } // Информация о материале
        public ForWhoDto? ForWho { get; set; } // Информация о том, для кого предназначен продукт
        public List<InsertionsDetailDto> InsertionsDetails { get; set; } = new List<InsertionsDetailDto>();
    }

}
