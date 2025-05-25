using System;
using System.Collections.Generic;

namespace JewelryStore_Prokofeva.ModelsDb;

public partial class JewelrySize
{
    public int Id { get; set; }

    public int? JewelryItemId { get; set; }

    public int? Size { get; set; }

    public int? StockQuantity { get; set; }

    public virtual JewelryItem? JewelryItem { get; set; }
}
