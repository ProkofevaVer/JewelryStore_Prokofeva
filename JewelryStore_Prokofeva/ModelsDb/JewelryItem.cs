using System;
using System.Collections.Generic;

namespace JewelryStore_Prokofeva.ModelsDb;

public partial class JewelryItem
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public decimal? Price { get; set; }

    public int? Discount { get; set; }

    public decimal? PriceDiscounr { get; set; }

    public float? Rating { get; set; }

    public string? Articul { get; set; }

    public string? PhotoUrl { get; set; }

    public int? CategoryId { get; set; }

    public int? MaterialId { get; set; }

    public int? ForWhoId { get; set; }

    public int? StockQuantity { get; set; }

    public float? ApproximateWeight { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ForWho? ForWho { get; set; }

    public virtual ICollection<InsertionsDetail> InsertionsDetails { get; set; } = new List<InsertionsDetail>();

    public virtual ICollection<JewelrySize> JewelrySizes { get; set; } = new List<JewelrySize>();

    public virtual Material? Material { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
