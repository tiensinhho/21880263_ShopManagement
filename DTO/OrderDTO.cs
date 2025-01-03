using _21880263.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21880263.DTO
{
    public class OrderDTO : ICloneable
    {
        public OrderDTO() { }

        public object Clone()
        {
            return MemberwiseClone();
        }
        public int OrderId { get; set; }

        public DateTime? OrderDate { get; set; }

        public string? OrderName { get; set; }

        public string? OrderPhone { get; set; }

        public string? OrderAddress { get; set; }

        public decimal? OrderTotal { get; set; }

        public bool OrderIsdeleted { get; set; }

        public virtual ObservableCollection<OrderDetailDTO> OrderDetails { get; set; } = new ObservableCollection<OrderDetailDTO>();

    }

    public class OrderDetailDTO : ICloneable
    {
        public object Clone()
        {
            return MemberwiseClone();
        }
        public int OrderdetailId { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }
        public string? ProductName { get; set; }

        public decimal? OrderdetailPrice { get; set; }

        public int? OrderdetailQuantity { get; set; }

        public bool OrderdetailIsdeleted { get; set; }

    }

}
