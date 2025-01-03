using _21880263.DAO;
using _21880263.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21880263.BUS
{
    public class ImportOrderBUS : OrderBUS
    {
        protected ObservableCollection<ImportOrder>? _orders { get; set; }
        protected ObservableCollection<ImportOrderDetail>? _details { get; set; }
        public ImportOrderBUS()
        {
            var imap = _configMapper();
            _orders = new ObservableCollection<ImportOrder>(EShopDbContext.Instance.ImportOrders.Include(x => x.ImportOrderDetails).Where(x => x.OrderIsdeleted == false).ToList());
            _details = new ObservableCollection<ImportOrderDetail>(EShopDbContext.Instance.ImportOrderDetails.ToList());
            Orders = imap.Map<ObservableCollection<ImportOrder>, ObservableCollection<OrderDTO>>(_orders);
            _orders = imap.Map<ObservableCollection<OrderDTO>, ObservableCollection<ImportOrder>>(Orders);
            _detailsDTO = imap.Map<ObservableCollection<ImportOrderDetail>, ObservableCollection<OrderDetailDTO>>(_details);
            _details = imap.Map<ObservableCollection<OrderDetailDTO>, ObservableCollection<ImportOrderDetail>>(_detailsDTO);
        }
        public void Write(OrderDTO order)
        {
            var imap = _configMapper();
            var _order = EShopDbContext.Instance.ImportOrders.FirstOrDefault(x => x.OrderId == order.OrderId);
            if (_order == null)
            {
                _order = imap.Map<OrderDTO,ImportOrder>(order);
                EShopDbContext.Instance.ImportOrders.Add(_order);
                EShopDbContext.Instance.SaveChanges();
                if (Orders != null) Orders.Add(order);
            }
            else
            {
                foreach (var i in _order.ImportOrderDetails)
                {
                    EShopDbContext.Instance.ImportOrderDetails.Remove(i);
                }

                foreach (var item in order.OrderDetails)
                {
                    var detail = imap.Map<OrderDetailDTO, ImportOrderDetail>(item);
                    EShopDbContext.Instance.ImportOrderDetails.Add(detail);
                }
                EShopDbContext.Instance.SaveChanges();
                if (Orders != null)
                {
                    var oldOrder = Orders.FirstOrDefault(x => x.OrderId == _order.OrderId);
                    if (oldOrder != null) { Orders[Orders.IndexOf(oldOrder)] = order; }
                }
            }

        }
        public void Remove(OrderDTO order)
        {
            var _order = EShopDbContext.Instance.ImportOrders.FirstOrDefault(x => x.OrderId == order.OrderId);
            if (_order != null)
            {
                _order.OrderIsdeleted = true;

                foreach (var item in _order.ImportOrderDetails)
                {
                    item.OrderdetailIsdeleted = true;
                }
                EShopDbContext.Instance.SaveChanges();
                _order = EShopDbContext.Instance.ImportOrders.FirstOrDefault(x => x.OrderId == order.OrderId);
                if (_order != null && Orders != null)
                {
                    var __order = Orders.FirstOrDefault(x => x.OrderId == order.OrderId);
                    if (__order != null) Orders.Remove(__order);
                }
            }
        }
    }
}
