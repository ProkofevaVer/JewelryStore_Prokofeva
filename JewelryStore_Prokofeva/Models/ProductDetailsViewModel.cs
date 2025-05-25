using Api_JewelryStore.Models.DTO_Models;

namespace JewelryStore_Prokofeva.Models
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<JewelrySizeDto> Sizes { get; set; }
    }

}
