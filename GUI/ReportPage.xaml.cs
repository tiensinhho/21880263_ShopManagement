using _21880263.BUS;
using LiveCharts.Wpf.Charts.Base;
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
using System.Runtime.Serialization;
using _21880263.GUI;

namespace _21880263
{
    /// <summary>
    /// Interaction logic for ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        ReportRevenuePage revenuePage = new ReportRevenuePage();
        ReportStockPage stockPage = new ReportStockPage();
        public ReportPage()
        {
            InitializeComponent();
            mainReportPage.Navigate(revenuePage);
        }

        private void RevenueTab_Click(object sender, RoutedEventArgs e)
        {
            mainReportPage.Navigate(revenuePage);
        }

        private void StockTab_Click(object sender, RoutedEventArgs e)
        {
            mainReportPage.Navigate(stockPage);
        }
    }
}
