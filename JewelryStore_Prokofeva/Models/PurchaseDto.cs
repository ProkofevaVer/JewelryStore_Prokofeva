using Api_JewelryStore.Models.DTO_Models;

namespace JewelryStore_Prokofeva.Models
{
    public class PurchaseDto
    {
        public int Id { get; set; }             // Идентификатор покупки
        public int UserId { get; set; }         // Идентификатор пользователя
        public int Quantity { get; set; }        // Количество
        public decimal TotalPrice { get; set; }  // Общая цена
        public DateTime PurchaseDate { get; set; } // Дата покупки
        public JewelrySizeDto JewelrySize { get; set; }
        public List<InsertionsDetailDto> InsertionsDetails { get; set; } = new List<InsertionsDetailDto>();
    }
}
