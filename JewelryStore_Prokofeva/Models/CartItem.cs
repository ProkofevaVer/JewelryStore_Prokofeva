namespace JewelryStore_Prokofeva.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int? JewelrySizesItemId { get; set; }

        public int? UserId { get; set; }

        public string? Status { get; set; }

        public int? CardQuantity { get; set; }

        public decimal? CardTotalPrice { get; set; }

        public DateOnly? CardDate { get; set; }
        // Конструктор класса
        public CartItem()
        {
            Status = "в корзине"; // Устанавливаем статус по умолчанию
            CardDate = DateOnly.FromDateTime(DateTime.Now); // Устанавливаем текущую дату
        }
    }
}
