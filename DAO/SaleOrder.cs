using System;
using System.Collections.Generic;

namespace _21880263.DAO;

public partial class SaleOrder
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public string? OrderName { get; set; }

    public string? OrderPhone { get; set; }

    public string? OrderAddress { get; set; }

    public decimal? OrderTotal { get; set; }

    public bool OrderIsdeleted { get; set; }

    public virtual ICollection<SaleOrderDetail> SaleOrderDetails { get; set; } = new List<SaleOrderDetail>();
}
