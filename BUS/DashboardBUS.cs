using _21880263.DAO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21880263.BUS
{
    public class DashboardBUS
    {
        public static int ProductCount()
        {
            return EShopDbContext.Instance.Products.Count();
        }
        public static DataTable RecentOrders()
        {
            var dt = new DataTable();
            var _query = EShopDbContext.Instance.SaleOrders.Where(s => s.OrderIsdeleted == false).OrderByDescending(s => s.OrderId).Take(5).ToList();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Customer", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("Total", typeof(decimal));
            foreach (var item in _query)
            {
                dt.Rows.Add(item.OrderId, item.OrderDate.ToString("MM/dd/yyyy"), item.OrderName, item.OrderPhone, item.OrderTotal);
            }
            return dt;
        }
        public static DataTable CategorySaleCount()
        {
            var dt = new DataTable();
            // Lấy ngày đầu tiên của tháng hiện tại
            var firstDayOfCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Ngày đầu tiên của tháng trước
            var firstDayOfLastMonth = firstDayOfCurrentMonth.AddMonths(-1);

            // Ngày đầu tiên của tháng hiện tại
            var lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);

            // Lấy các hóa đơn từ tháng trước
            var lastMonthOrders = EShopDbContext.Instance.SaleOrders.Include(s => s.SaleOrderDetails)
                .Where(o => o.OrderDate >= firstDayOfLastMonth && o.OrderDate <= lastDayOfLastMonth && !o.OrderIsdeleted)
                .ToList();
            var categories = EShopDbContext.Instance.Categories.Include(s => s.Products).Where(c => !c.CategoryIsdeleted).ToList();
            dt.Columns.Add("Month", typeof(string));
            dt.Columns.Add("Year", typeof(string));
            dt.Columns.Add("Category", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            foreach (var c in categories)
            {
                int q = 0;
                foreach (var p in c.Products)
                {
                    foreach (var o in lastMonthOrders)
                    {
                        foreach (var s in o.SaleOrderDetails)
                        {
                            if (p.ProductId == s.ProductId)
                            {
                                if (s.OrderdetailQuantity!= null) q = (int)(q + s.OrderdetailQuantity);
                            }
                        }
                    }
                }
                dt.Rows.Add(lastDayOfLastMonth.Month, lastDayOfLastMonth.Year,c.CategoryName,q);
            }
            return dt;
        }
    }
}
