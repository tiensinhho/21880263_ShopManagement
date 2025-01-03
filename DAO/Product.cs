using System;
using System.Collections.Generic;

namespace _21880263.DAO;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? ProductDescription { get; set; }

    public int? CategoryId { get; set; }

    public int? BrandId { get; set; }

    public decimal? ProductSellprice { get; set; }

    public decimal? ProductImportprice { get; set; }

    public int? ProductQuantity { get; set; }

    public bool ProductIsdeleted { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Image? Image { get; set; }

    public virtual ICollection<ImportOrderDetail> ImportOrderDetails { get; set; } = new List<ImportOrderDetail>();

    public virtual ICollection<SaleOrderDetail> SaleOrderDetails { get; set; } = new List<SaleOrderDetail>();
}
