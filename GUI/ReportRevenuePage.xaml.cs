using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using _21880263.BUS;

namespace _21880263.GUI
{
    /// <summary>
    /// Interaction logic for ReportRevenuePage.xaml
    /// </summary>
    public partial class ReportRevenuePage : Page
    {
        public ReportRevenuePage()
        {
            InitializeComponent();
            _loadCombobox();
            _loadStock();
            _revenue();
        }
        void _loadStock()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (FromCombobox.SelectedItem !=null && ToCombobox.SelectedItem != null)
                {
                    DateTime start = (DateTime)((ComboBoxItem)FromCombobox.SelectedItem).Tag;
                    DateTime end = (DateTime)((ComboBoxItem)ToCombobox.SelectedItem).Tag;
                    StockTable.ItemsSource = ReportBUS.MonthlyRevenue(start, end).AsDataView();
                }
                else
                {
                    StockTable.ItemsSource = ReportBUS.MonthlyRevenue().AsDataView();

                }
                StockTable.IsReadOnly = true;
                StockTable.CanUserResizeColumns = false;
            }));
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
        }
        void _revenue()
        {
            ChartValues<double> _revenue, _sale, _import;
            _revenue = new ChartValues<double>();
            _sale = new ChartValues<double>();
            _import = new ChartValues<double>();
            List<string> label;
            label = new List<string>();
            FromCombobox.SelectedIndex = (FromCombobox.Items.Count - 10) < 0 ? 0 : (FromCombobox.Items.Count - 10);
            ToCombobox.SelectedIndex = ToCombobox.Items.Count - 1;
            if (FromCombobox.SelectedItem != null && ToCombobox.SelectedItem != null)
            {
                DateTime start = (DateTime)((ComboBoxItem)FromCombobox.SelectedItem).Tag;
                DateTime end = (DateTime)((ComboBoxItem)ToCombobox.SelectedItem).Tag;
                _reloadChart(_revenue, _sale, _import, label, start, end);
            }
        }

        private void _reloadChart(ChartValues<double> _revenue, ChartValues<double> _sale, ChartValues<double> _import, List<string> label, DateTime start, DateTime end)
        {
            double _revenuetotal = 0;
            foreach (DataRow row in ReportBUS.MonthlyRevenue(start, end).Rows)
            {
                var checkpoint = new DateTime((int)row["Year"], (int)row["Month"], 1);
                if (start <= checkpoint && checkpoint < end)
                {
                    _revenue.Add((double)row["Revenue"]);
                    _revenuetotal += (double)row["Revenue"];
                    _sale.Add((double)row["Sales"]);
                    _import.Add((double)row["Import"]);
                    label.Add(row["Month"].ToString() + "/" + row["Year"]);
                }
            }
            totalRevenue.Text = _revenuetotal.ToString();

            // Tạo một đối tượng SeriesCollection và thêm dữ liệu vào đó
            SeriesCollection series = new SeriesCollection();
            series.Add(new LineSeries { Values = _revenue });

            // Gán dữ liệu cho đồ thị
            Revenue.Series = new SeriesCollection()
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
                    Fill = new SolidColorBrush(Color.FromArgb(0,255,255,255)),
                },
                new LineSeries()
                {
                    Title = "Import",
                    Values = _import,
                    Fill = new SolidColorBrush(Color.FromArgb(0,255,255,255)),
                }

            };
            if(Revenue.AxisX.Count == 0)
            {
                Revenue.AxisX.Add(new Axis()
                {
                    Title = "Month",
                    Labels = label
                });
            }
            else
            {
                Revenue.AxisX[0] = (new Axis()
                {
                    Title = "Month",
                    Labels = label
                });
            }
        }

        private void _loadCombobox()
        {
            foreach (DataRow row in ReportBUS.MonthlyRevenue().Rows)
            {
                ComboBoxItem fromItem = new ComboBoxItem()
                {
                    Content = (new DateTime((int)row[1], (int)row[0], 1)).ToString("MMM-yyyy"),
                    Tag = new DateTime((int)row[1], (int)row[0], 1)
                };
                FromCombobox.Items.Add(fromItem);
                ComboBoxItem toItem = new ComboBoxItem()
                {
                    Content = (new DateTime((int)row[1], (int)row[0], 1)).AddMonths(1).AddDays(-1).ToString("MMM-yyyy"),
                    Tag = (new DateTime((int)row[1], (int)row[0], 1)).AddMonths(1)
                };

                ToCombobox.Items.Add(toItem);
            }
        }

        private void FromCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _loadStock();
            ChartValues<double> _revenue, _sale, _import;
            _revenue = new ChartValues<double>();
            _sale = new ChartValues<double>();
            _import = new ChartValues<double>();
            List<string> label = new List<string>();
            if (FromCombobox.SelectedItem != null && ToCombobox.SelectedItem != null)
            {
                DateTime start = (DateTime)((ComboBoxItem)FromCombobox.SelectedItem).Tag;
                DateTime end = (DateTime)((ComboBoxItem)ToCombobox.SelectedItem).Tag;
                _reloadChart(_revenue, _sale, _import, label, start, end);
            }
        }

        private void ToCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _loadStock();
            ChartValues<double> _revenue, _sale, _import;
            _revenue = new ChartValues<double>();
            _sale = new ChartValues<double>();
            _import = new ChartValues<double>();
            List<string> label = new List<string>();
            if (FromCombobox.SelectedItem != null && ToCombobox.SelectedItem != null)
            {
                DateTime start = (DateTime)((ComboBoxItem)FromCombobox.SelectedItem).Tag;
                DateTime end = (DateTime)((ComboBoxItem)ToCombobox.SelectedItem).Tag;
                _reloadChart(_revenue, _sale, _import, label, start, end);
            }
        }
    }
}
