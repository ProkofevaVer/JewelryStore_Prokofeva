using System;
using System.Collections.Generic;

namespace JewelryStore_Prokofeva.ModelsDb;

public partial class Purchase
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? JewelryItemsId { get; set; }

    public int? Quantity { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateOnly? PurchaseDate { get; set; }

    public virtual JewelryItem? JewelryItems { get; set; }

    public virtual User? User { get; set; }
}
