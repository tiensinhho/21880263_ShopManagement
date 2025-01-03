using System;
using System.Collections.Generic;

namespace _21880263.DAO;

public partial class Category : ICloneable
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? CategoryDescription { get; set; }

    public bool CategoryIsdeleted { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public object Clone()
    {
        return MemberwiseClone();
    }
}
