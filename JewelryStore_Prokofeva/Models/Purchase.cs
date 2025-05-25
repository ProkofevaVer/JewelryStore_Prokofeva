using System;
using System.Collections.Generic;

namespace Api_JewelryStore.Models;

public partial class Purchase
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? JewelryItemsSizeId { get; set; }

    public int? Quantity { get; set; }

    public decimal? TotalPrice { get; set; }

    public DateOnly? PurchaseDate { get; set; }
}
