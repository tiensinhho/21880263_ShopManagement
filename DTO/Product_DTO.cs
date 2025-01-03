using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.ComponentModel;
using _21880263.DAO;

namespace _21880263.DTO
{
    public class Product_DTO :  ICloneable
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public BitmapImage? ProductImage { get; set; }

        public string? ProductDescription { get; set; }

        public int? CategoryId { get; set; }

        public int? BrandId { get; set; }

        public decimal? ProductSellprice { get; set; }

        public decimal? ProductImportprice { get; set; }

        public int? ProductQuantity { get; set; }

        public virtual Brand? Brand { get; set; }

        public virtual Category? Category { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
