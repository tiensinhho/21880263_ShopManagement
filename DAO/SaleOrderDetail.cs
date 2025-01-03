using System;
using System.Collections.Generic;

namespace _21880263.DAO;

public partial class SaleOrderDetail
{
    public int OrderdetailId { get; set; }

    public int OrderId { get; set; }

    public int? ProductId { get; set; }

    public decimal? OrderdetailPrice { get; set; }

    public int? OrderdetailQuantity { get; set; }

    public bool OrderdetailIsdeleted { get; set; }

    public virtual SaleOrder Order { get; set; } = null!;

    public virtual Product? Product { get; set; }
}
