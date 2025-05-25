namespace JewelryStore_Prokofeva.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public decimal? Money { get; set; }

        public bool IsAdmin { get; set; }
    }
}
