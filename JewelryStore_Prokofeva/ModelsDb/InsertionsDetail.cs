using System;
using System.Collections.Generic;

namespace JewelryStore_Prokofeva.ModelsDb;

public partial class InsertionsDetail
{
    public int Id { get; set; }

    public int? JewelryItemId { get; set; }

    public int? InsertionId { get; set; }

    public int? Quantity { get; set; }

    public virtual Insertion? Insertion { get; set; }

    public virtual ICollection<InsertionsCharacteristic> InsertionsCharacteristics { get; set; } = new List<InsertionsCharacteristic>();

    public virtual JewelryItem? JewelryItem { get; set; }
}
