using System;
using System.Collections.Generic;

namespace _21880263.DAO;

public partial class ImportOrderDetail
{
    public int OrderdetailId { get; set; }

    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public decimal? OrderdetailPrice { get; set; }

    public int? OrderdetailQuantity { get; set; }

    public bool OrderdetailIsdeleted { get; set; }

    public virtual ImportOrder? Order { get; set; }

    public virtual Product? Product { get; set; }
}
