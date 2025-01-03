using System;
using System.Collections.Generic;

namespace _21880263.DAO;

public partial class Brand: ICloneable
{
    public int BrandId { get; set; }

    public string BrandName { get; set; } = null!;

    public string? BrandDescription { get; set; }

    public bool BrandIsdeleted { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public object Clone()
    {
        return MemberwiseClone();
    }
}
