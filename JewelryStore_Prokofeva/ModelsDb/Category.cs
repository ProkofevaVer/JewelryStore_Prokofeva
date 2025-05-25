using System;
using System.Collections.Generic;

namespace JewelryStore_Prokofeva.ModelsDb;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<JewelryItem> JewelryItems { get; set; } = new List<JewelryItem>();
}
