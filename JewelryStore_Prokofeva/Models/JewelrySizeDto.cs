namespace Api_JewelryStore.Models.DTO_Models
{
    public class JewelrySizeDto
    {
        public int Id { get; set; }

        public int? JewelryItemId { get; set; }

        public int? Size { get; set; }

        public int? StockQuantity { get; set; }
        public string SizeName => Size.HasValue ? Size.Value.ToString() : "Unknown";

        public JewelryItemDto? JewelryItem { get; set; } // Информация о ювелирном изделии
    }
}
