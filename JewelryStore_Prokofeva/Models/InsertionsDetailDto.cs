namespace Api_JewelryStore.Models.DTO_Models
{
    public class InsertionsDetailDto
    {
        public int Id { get; set; }
        public int? Quantity { get; set; }
        public InsertionDto? Insertion { get; set; } // Информация о вставке
        public List<InsertionsCharacteristicDto> InsertionsCharacteristics { get; set; } = new List<InsertionsCharacteristicDto>();
    }

}
