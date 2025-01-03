using System;
using System.Collections.Generic;

namespace _21880263.DAO;

public partial class Image : ICloneable
{
    public int ProductId { get; set; }

    public byte[]? ProductImage { get; set; }

    public virtual Product Product { get; set; } = null!;

    public object Clone()
    {
        return MemberwiseClone();
    }
}
