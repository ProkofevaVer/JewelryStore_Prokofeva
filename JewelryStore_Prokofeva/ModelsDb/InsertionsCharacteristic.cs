using System;
using System.Collections.Generic;

namespace JewelryStore_Prokofeva.ModelsDb;

public partial class InsertionsCharacteristic
{
    public int Id { get; set; }

    public int? InsertionsDetailsId { get; set; }

    public string? ShapeForm { get; set; }

    public string? Color { get; set; }

    public string? Dimensions { get; set; }

    public int? Clarity { get; set; }

    public int? CutOgranka { get; set; }

    public int? ColorGrade { get; set; }

    public float? WeightCarat { get; set; }

    public virtual InsertionsDetail? InsertionsDetails { get; set; }
}
