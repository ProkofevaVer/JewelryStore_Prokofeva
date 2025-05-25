using Api_JewelryStore.Models.DTO_Models;

namespace JewelryStore_Prokofeva.Models
{
    public class Product
    {
        public int Id { get; set; } 
        public string Title { get; set; } 
        public decimal? Price { get; set; } 
        public int? Discount { get; set; } 
        public decimal? PriceDiscounr { get; set; } 
        public string Articul { get; set; } 
        public string PhotoUrl { get; set; } 
        public int? StockQuantity { get; set; }

        public float? ApproximateWeight { get; set; }
        public List<JewelrySizeDto> JewelrySizes { get; set; } 
        public CategoryDto Category { get; set; } 
        public MaterialDto Material { get; set; }
        public ForWhoDto ForWho { get; set; }
        public float? Rating { get; set; }
        public List<InsertionsDetailDto> InsertionsDetails { get; set; } 

        public Product()
        {
            InsertionsDetails = new List<InsertionsDetailDto>();
            JewelrySizes = new List<JewelrySizeDto>();
        }
    }

}
