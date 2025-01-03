using _21880263.DAO;
using MaterialDesignThemes.Wpf.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21880263.BUS
{
    public class ReportBUS
    {
        public static DataTable Stock() 
        { 
            var _query = EShopDbContext.Instance.Products.Select(row => new {ProductName = row.ProductName, Quantity = row.ProductQuantity }).ToList();
            var datatable = new DataTable();
            datatable.Columns.Add("Product Name",typeof(string));
            datatable.Columns.Add("Stock",typeof(int));
            datatable.Columns.Add("Satus",typeof(string));
            foreach (var row in _query)
            {
                datatable.Rows.Add( row.ProductName, row.Quantity, row.Quantity==0 ? "out of stock":"stocking");
            }
            return datatable; 
        }

        public static DataTable CategoryReport()
        {
            var dt = new DataTable();
            var _query = EShopDbContext.Instance.Categories.Select(x => new {x.CategoryName, Quantity = x.Products.Sum(p => p.ProductQuantity) }).ToList();
            dt.Columns.Add("Category",typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            foreach (var i in _query)
            {
                dt.Rows.Add(i.CategoryName, i.Quantity);
            }
            return dt;
        }
        public static DataTable BestSellerReport()
        {
            var dt = new DataTable();
            var _query = EShopDbContext.Instance.Products.OrderByDescending(p => p.SaleOrderDetails.Sum(x => x.OrderdetailQuantity)).Take(10).Select(x => new { x.ProductName, Quantity = x.SaleOrderDetails.Sum(p => p.OrderdetailQuantity)}).ToList();
            dt.Columns.Add("Product", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            foreach (var i in _query)
            {
                dt.Rows.Add(i.ProductName, i.Quantity);
            }
            return dt;
        }
        public static DataTable UnderperformingReport()
        {
            var dt = new DataTable();
            var _query = EShopDbContext.Instance.Products.OrderBy(p => p.SaleOrderDetails.Sum(x => x.OrderdetailQuantity)).Take(10).Select(x => new { x.ProductName, Quantity = x.SaleOrderDetails.Sum(p => p.OrderdetailQuantity) }).ToList();
            dt.Columns.Add("Product", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            foreach (var i in _query)
            {
                dt.Rows.Add(i.ProductName, i.Quantity);
            }
            return dt;
        }

        public static DataTable MonthlyRevenue(DateTime from , DateTime to)
        { 
            var _salequery = EShopDbContext.Instance.SaleOrders.Where(x => from < x.OrderDate && x.OrderDate <to ).GroupBy(s => new { s.OrderDate.Month, s.OrderDate.Year }).Select(s => new {Month = s.Key.Month, Year =s.Key.Year, Sales = s.Sum(s => s.OrderTotal)}).ToList();
            var _importquery = EShopDbContext.Instance.ImportOrders.Where(x => from < x.OrderDate && x.OrderDate < to).GroupBy(s => new { s.OrderDate.Month, s.OrderDate.Year }).Select(s => new { Month = s.Key.Month, Year = s.Key.Year, Import = s.Sum(s => s.OrderTotal) }).ToList();
            var datatable = new DataTable();
            datatable.Columns.Add("Month", typeof(int));
            datatable.Columns.Add("Year", typeof(int));
            datatable.Columns.Add("Sales", typeof(double));
            datatable.Columns.Add("Import", typeof(double));
            datatable.Columns.Add("Revenue", typeof(double));
            foreach (var sale in _salequery)
            {
                foreach (var import in _importquery)
                {
                    if (sale.Month == import.Month && sale.Year == import.Year)
                    datatable.Rows.Add(sale.Month, sale.Year, sale.Sales, import.Import, sale.Sales - import.Import);
                }
            }
            return datatable;
        }
        public static DataTable MonthlyRevenue()
        {
            var _salequery = EShopDbContext.Instance.SaleOrders.GroupBy(s => new { s.OrderDate.Month, s.OrderDate.Year }).Select(s => new { Month = s.Key.Month, Year = s.Key.Year, Sales = s.Sum(s => s.OrderTotal) }).ToList();
            var _importquery = EShopDbContext.Instance.ImportOrders.GroupBy(s => new {s.OrderDate.Month, s.OrderDate.Year }).Select(s => new { Month = s.Key.Month, Year = s.Key.Year, Import = s.Sum(s => s.OrderTotal) }).ToList();
            var datatable = new DataTable();
            datatable.Columns.Add("Month", typeof(int));
            datatable.Columns.Add("Year", typeof(int));
            datatable.Columns.Add("Sales", typeof(double));
            datatable.Columns.Add("Import", typeof(double));
            datatable.Columns.Add("Revenue", typeof(double));
            foreach (var sale in _salequery)
            {
                foreach (var import in _importquery)
                {
                    if (sale.Month == import.Month && sale.Year == import.Year)
                        datatable.Rows.Add(sale.Month, sale.Year, sale.Sales, import.Import, sale.Sales - import.Import);
                }
            }
            return datatable;
        }

    }
}
