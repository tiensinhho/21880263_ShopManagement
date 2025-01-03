using _21880263.DAO;
using _21880263.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace _21880263.BUS
{
    public class SaleOrdersBUS : OrderBUS
    {
        protected ObservableCollection<SaleOrder>? _orders { get; set; }
        protected ObservableCollection<SaleOrderDetail>? _details { get; set; }

        public SaleOrdersBUS()
        {
            var imap = _configMapper();
            _orders = new ObservableCollection<SaleOrder>(EShopDbContext.Instance.SaleOrders.Include(x => x.SaleOrderDetails).Where(x => x.OrderIsdeleted == false).ToList());
            _details = new ObservableCollection<SaleOrderDetail>(EShopDbContext.Instance.SaleOrderDetails.ToList());
            Orders = imap.Map<ObservableCollection<SaleOrder>, ObservableCollection<OrderDTO>>(_orders);
            _orders = imap.Map<ObservableCollection<OrderDTO>, ObservableCollection<SaleOrder>>(Orders);
        }
        public void Write(OrderDTO order)
        {
            var imap = _configMapper();
            var _order = EShopDbContext.Instance.SaleOrders.FirstOrDefault(x => x.OrderId == order.OrderId);
            if (_order == null)
            {
                _order = imap.Map<OrderDTO,SaleOrder>(order);
                EShopDbContext.Instance.SaleOrders.Add(_order);
                EShopDbContext.Instance.SaveChanges();
                if (Orders != null) Orders.Add(order);
            }
            else
            {
                foreach (var i in _order.SaleOrderDetails)
                {
                    EShopDbContext.Instance.SaleOrderDetails.Remove(i);
                }

                foreach (var item in order.OrderDetails)
                {
                    var detail = imap.Map<OrderDetailDTO,SaleOrderDetail>(item);
                    EShopDbContext.Instance.SaleOrderDetails.Add(detail);
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
            var _order = EShopDbContext.Instance.SaleOrders.FirstOrDefault(x => x.OrderId == order.OrderId);
            if (_order != null)
            {
                _order.OrderIsdeleted = true;
                foreach (var item in _order.SaleOrderDetails)
                {
                    item.OrderdetailIsdeleted = true;
                }
                EShopDbContext.Instance.SaveChanges();
                _order = EShopDbContext.Instance.SaleOrders.FirstOrDefault(x => x.OrderId == order.OrderId);
                if (_order != null)
                {
                    if (Orders != null)
                    {
                        var __order = Orders.FirstOrDefault(x => x.OrderId == order.OrderId);
                        if (__order != null) Orders.Remove(__order);
                    }
                }
            }
        }
    }
}
