using _21880263.BUS;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _21880263.GUI
{
    /// <summary>
    /// Interaction logic for ReportStockPage.xaml
    /// </summary>
    public partial class ReportStockPage : Page
    {
        public ReportStockPage()
        {
            InitializeComponent();
            _loadData();
        }

        private void _loadData()
        {
            if (productStockDataGrid != null) { productStockDataGrid.ItemsSource = ReportBUS.Stock().AsDataView(); }
            if (categoryPieChart != null) 
            {
                var series = new SeriesCollection();
                foreach (DataRow row in ReportBUS.CategoryReport().Rows)
                {
                    series.Add(new PieSeries()
                    {
                        Title = row["Category"].ToString(),
                        Values = new ChartValues<int>{ (int)row["Quantity"] },
                        DataLabels = true,
                    });
                }
                categoryPieChart.Series = series;
            }
            if (bestsellerChart != null)
            {
                var series = new SeriesCollection();
                foreach (DataRow row in ReportBUS.BestSellerReport().Rows)
                {
                    series.Add(new RowSeries()
                    {
                        Title = row["Product"].ToString(),
                        Values = new ChartValues<int> { (int)row["Quantity"] },
                        DataLabels = true,
                    });
                }
                bestsellerChart.Series = series;
            }
            if (underperformingChart != null)
            {
                var series = new SeriesCollection();
                foreach (DataRow row in ReportBUS.UnderperformingReport().Rows)
                {
                    series.Add(new RowSeries()
                    {
                        Title = row["Product"].ToString(),
                        Values = new ChartValues<int> { (int)row["Quantity"] },
                        DataLabels = true,
                    });
                }
                underperformingChart.Series = series;

            }
        }
        private void StockTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Product Name")
            {
                e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
            if (e.PropertyName == "ID")
            {
                e.Column.Width = new DataGridLength(50);
            }
            if (e.PropertyName == "Satus")
            {
                e.Column.Width = new DataGridLength(100);
            }

        }

        private void PieChart_DataClick(object sender, LiveCharts.ChartPoint chartPoint)
        {
            var chart = chartPoint.ChartView as PieChart;
            if (chart != null)
            {
                foreach (PieSeries series in chart.Series) series.PushOut = 0;
            }
            var selectSeries = chartPoint.SeriesView as PieSeries;
            if(selectSeries != null)selectSeries.PushOut = 15;
        }
    }
}
