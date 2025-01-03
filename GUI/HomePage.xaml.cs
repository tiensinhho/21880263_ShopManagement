using _21880263.BUS;
using _21880263.DTO;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _21880263
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _loadData();
        }

        private void _loadData()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (productTotalLabel != null) productTotalLabel.Content = DashboardBUS.ProductCount();
                if (recentOrdersDatagrid != null)
                {
                    recentOrdersDatagrid.ItemsSource = DashboardBUS.RecentOrders().AsDataView();
                    recentOrdersDatagrid.IsReadOnly = true;
                }
                if (countProductsByCategoryLastMonthChart != null)
                {
                    var series = new SeriesCollection();
                    string lable = string.Empty;
                    foreach (DataRow row in DashboardBUS.CategorySaleCount().Rows)
                    {
                        if (string.IsNullOrEmpty(lable)) { lable = row[0].ToString() + "/" + row[1].ToString(); }
                        series.Add(new PieSeries()
                        {
                            Title = row["Category"].ToString(),
                            Values = new ChartValues<int> { (int)row["Quantity"] },
                            DataLabels = true,
                        });
                    }
                    countProductsByCategoryLastMonthChart.Series = series;
                    countCategoryLabel.Content = lable;
                }
                if (revenue2month != null)
                {
                    var series = new SeriesCollection();
                    List<string> label = new List<string>();
                    var _revenue = new ChartValues<double>();
                    var _sale = new ChartValues<double>();
                    var _import = new ChartValues<double>();
                    var dt = new DataTable();
                    // Lấy ngày đầu tiên của tháng hiện tại
                    var firstDayOfCurrentMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                    // Ngày đầu tiên của tháng trước
                    var firstDayOfLastMonth = firstDayOfCurrentMonth.AddMonths(-2);
                    var data = ReportBUS.MonthlyRevenue(firstDayOfLastMonth, firstDayOfCurrentMonth);
                    double _revenuetotal = 0;
                    foreach (DataRow row in data.Rows)
                    {
                        _revenue.Add((double)row["Revenue"]);
                        _revenuetotal += (double)row["Revenue"];
                        _sale.Add((double)row["Sales"]);
                        _import.Add((double)row["Import"]);
                        label.Add(row["Month"].ToString() + "/" + row["Year"]);
                    }
                    totalRevenue.Content = _revenuetotal.ToString();

                    // Tạo một đối tượng SeriesCollection và thêm dữ liệu vào đó
                    SeriesCollection _series = new SeriesCollection();
                    series.Add(new LineSeries { Values = _revenue });

                    // Gán dữ liệu cho đồ thị
                    revenue2month.Series = new SeriesCollection()
            {
                new ColumnSeries()
                {
                    Title = "Revenue",
                    Values = _revenue,
                },
                new LineSeries()
                {
                    Title = "Sales",
                    Values = _sale,
                    //Stroke = Brushes.Violet,
                    Fill = new SolidColorBrush(Color.FromArgb(0,255,255,255)),
                },
                new LineSeries()
                {
                    Title = "Import",
                    Values = _import,
                    //Stroke = Brushes.Green,
                    Fill = new SolidColorBrush(Color.FromArgb(0,255,255,255)),
                }

            };
                    if (revenue2month.AxisX.Count == 0)
                    {
                        revenue2month.AxisX.Add(new Axis()
                        {
                            Title = "Month",
                            Labels = label
                        });
                    }
                    else
                    {
                        revenue2month.AxisX[0] = (new Axis()
                        {
                            Title = "Month",
                            Labels = label
                        });
                    }

                }
                if (salelLabel != null && revenuelLabel != null)
                {
                    double _sale = 0;
                    double _revenue = 0;
                    var __data = ReportBUS.MonthlyRevenue();
                    foreach (DataRow item in __data.Rows)
                    {
                        _sale += (double)item["Sales"];
                        _revenue += (double)item["Revenue"];
                    }
                    salelLabel.Content = "$" + _sale;
                    revenuelLabel.Content = "$" + _revenue;
                }
            }));
        }
        private void StockTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Customer")
            {
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            if (e.PropertyName == "ID")
            {
                e.Column.Width = new DataGridLength(60);
            }
        }

    }

}
