using _21880263.DAO;
using _21880263.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21880263.BUS
{
    public class OrderBUS : OrderBUS_Interface, ICloneable
    {
        public object Clone()
        {
            return MemberwiseClone();
        }

        public ObservableCollection<OrderDTO>? Orders { get; set; }
        protected ObservableCollection<OrderDetailDTO>? _detailsDTO { get; set; }
        protected virtual IMapper _configMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDTO, SaleOrder>().ForMember(x => x.SaleOrderDetails, y => y.MapFrom(s => s.OrderDetails));
                cfg.CreateMap<OrderDTO, ImportOrder>().ForMember(x => x.ImportOrderDetails, y => y.MapFrom(s => s.OrderDetails));
                cfg.CreateMap<SaleOrder, OrderDTO>().ForMember(x => x.OrderDetails, y => y.MapFrom(s => s.SaleOrderDetails));
                cfg.CreateMap<ImportOrder, OrderDTO>().ForMember(x => x.OrderDetails, y => y.MapFrom(s => s.ImportOrderDetails));
                cfg.CreateMap<OrderDetailDTO, SaleOrderDetail>();
                cfg.CreateMap<OrderDetailDTO, ImportOrderDetail>();
                cfg.CreateMap<SaleOrderDetail, OrderDetailDTO>().ForMember(s => s.ProductName, f => f.MapFrom(src => src.Product != null ? src.Product.ProductName:string.Empty));
                cfg.CreateMap<ImportOrderDetail, OrderDetailDTO>().ForMember(s => s.ProductName, f => f.MapFrom(src => src.Product != null ? src.Product.ProductName : string.Empty));
            });
            // Tạo mapper từ cấu hình
            IMapper mapper = config.CreateMapper();
            return mapper;
        }
    }
    public interface OrderBUS_Interface
    {
        public virtual void Write(OrderDTO order)
        {
        }
        public virtual void Remove(OrderDTO order)
        {
        }
    }

}
