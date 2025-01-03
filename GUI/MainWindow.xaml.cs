using _21880263.DAO;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace _21880263
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string? UserID { get; set; }
        HomePage? _homePage { get; set; }
        ProductPage? _productPage { get; set; }
        OrderPage? _orderPage { get; set; }
        ReportPage? _reportPage { get; set; }

        public delegate void SearchHandler(string keyword);

        public event SearchHandler? SearchUpdating;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => {
                if (_homePage == null) _homePage = new HomePage();
                mainPage.Navigate(_homePage);
                if (_homePage != null) namePage.Content = _homePage.Title;
                if (!string.IsNullOrEmpty(UserID)) Account_Popup.Icon = UserID.First();
            });
            DataContext = this;
            loadProgressbar.Visibility = Visibility.Collapsed;
        }

        private void nav_button(object sender, RoutedEventArgs e)
        {
            dashboardLabel.Visibility = dashboardLabel.Visibility == Visibility.Collapsed? Visibility.Visible:Visibility.Collapsed;
            productLabel.Visibility = productLabel.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            orderLabel.Visibility = orderLabel.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            reportLabel.Visibility = reportLabel.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        private void account_buttonClick(object sender, RoutedEventArgs e)
        {
            if (account_popupBox.IsPopupOpen == true)
            {
                account_popupBox.IsPopupOpen = false;
            }
            else
            {
                account_popupBox.IsPopupOpen = true;
            }
        }

        private void homePage_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => {
                if (_homePage == null) _homePage = new HomePage();
                mainPage.Navigate(_homePage);
                namePage.Content = _homePage.Title;
            });
        }

        private void productPage_click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (_productPage == null) _productPage = new ProductPage(this);
                mainPage.Navigate(_productPage);
                namePage.Content = _productPage.Title;
            });
        }

        private void order_Button_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (_orderPage == null) _orderPage = new OrderPage();
                mainPage.Navigate(_orderPage);
                namePage.Content = _orderPage.Title;
            });
        }

        private void report_Button_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (_reportPage == null) _reportPage = new ReportPage();
                mainPage.Navigate(_reportPage);
                namePage.Content = _reportPage.Title;
            });
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher?.Invoke(() => {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["Password"].Value = "";
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                var window = new LoginWindow();
                window.Show();
                Hide();
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current?.Shutdown();
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void TitleBoder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            sentKeyword();
        }

        private void searchTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Dispatcher.Invoke(() =>
                {
                    sentKeyword();
                });
            }
        }
        private void sentKeyword()
        {
            SearchUpdating?.Invoke(searchTextbox.Text);
        }
    }
}
