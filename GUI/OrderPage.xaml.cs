using _21880263.BUS;
using _21880263.DAO;
using _21880263.DTO;
using _21880263.GUI;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using DataGridTextColumn = System.Windows.Controls.DataGridTextColumn;

namespace _21880263
{
    /// <summary>
    /// Interaction logic for OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        Product_BUS _product_BUS { get; set; } = new Product_BUS();
        SaleOrdersBUS _saleBUS { get; set; } = new SaleOrdersBUS();
        ImportOrderBUS _importBUS { get; set; } = new ImportOrderBUS();        
        OrderDTO? _order { get; set; }
        public OrderPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await _loadProducts();
            await _reloadOrderData();
        }

        Task _loadProducts()
        {
            Dispatcher.Invoke(() =>
            {
                Product_DataGrid.ItemsSource = _product_BUS.Products;
            });
            return Task.CompletedTask;
        }
        Task _reloadOrderData()
        {
            if (kindComboBox.SelectedIndex == 0) Order_DataGrid.ItemsSource = _saleBUS.Orders;
            else Order_DataGrid.ItemsSource = _importBUS.Orders;
            Total_lable.Text = "";
            idTextBox.Text = "";
            datePicker.Text = DateTime.Now.ToString("MM/dd/yyyy");
            customerTextbox.Text = "";
            phoneTextBox.Text = "";
            addressTextBox.Text = "";
            checkoutButton.Visibility = Visibility.Collapsed;
            cartaddButton.Visibility = Visibility.Collapsed;
            cartremoveButton.Visibility = Visibility.Collapsed;
            datePicker.IsEnabled = false;
            customerTextbox.IsReadOnly = true;
            phoneTextBox.IsReadOnly = true;
            addressTextBox.IsReadOnly = true;
            _order = new OrderDTO();
            _order.OrderDetails = new ObservableCollection<OrderDetailDTO> { new OrderDetailDTO() };
            Cart_DataGrid.ItemsSource = _order.OrderDetails;
            if (Order_DataGrid.Columns.Count > 0) return Task.CompletedTask;
                Dispatcher.Invoke(() =>
                {
                    if (Order_DataGrid.Columns.Count == 0)
                    {
                        Order_DataGrid.Columns.Add(new DataGridTextColumn()
                        {
                            Header = "ID",
                            Binding = new Binding("OrderId"),
                            MaxWidth = 200,
                        });
                        Order_DataGrid.Columns.Add(new DataGridTextColumn()
                        {
                            Header = "Date",
                            Binding = new Binding("OrderDate") { StringFormat = "MM/dd/yyyy" },
                            MaxWidth = 200,

                        });
                        Order_DataGrid.Columns.Add(new DataGridTextColumn()
                        {
                            Header = "Customer",
                            Binding = new Binding("OrderName"),
                            Width = new DataGridLength(100, DataGridLengthUnitType.Star),

                        });
                        Order_DataGrid.Columns.Add(new DataGridTextColumn()
                        {
                            Header = "Phone",
                            Binding = new Binding("OrderPhone"),
                            Width = 120,
                        });
                    }
                });
            return Task.CompletedTask;
        }

    private void Order_DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() => {
                checkoutButton.Visibility = Visibility.Collapsed;
                cartaddButton.Visibility = Visibility.Collapsed;
                cartremoveButton.Visibility = Visibility.Collapsed;
                customerTextbox.IsReadOnly = true;
                datePicker.IsEnabled = false;
                phoneTextBox.IsReadOnly = true;
                addressTextBox.IsReadOnly = true;
                Cart_DataGrid.Columns.Clear();
                Cart_DataGrid.IsReadOnly = true;
                Cart_DataGrid.CanUserDeleteRows = false;
                Cart_DataGrid.CanUserAddRows = false;
                _createColumnsCart();
                var __selected = ((OrderDTO)Order_DataGrid.SelectedItem);
                if (_saleBUS.Orders != null && _importBUS.Orders != null && __selected != null)
                {
                    var _details = (kindComboBox.SelectedIndex == 0) ? _saleBUS.Orders.FirstOrDefault(s => s.OrderId == __selected.OrderId) : _importBUS.Orders.FirstOrDefault(s => s.OrderId == __selected.OrderId);
                    if (_details != null)
                    {
                        Cart_DataGrid.ItemsSource = _details.OrderDetails;
                        Total_lable.Text = _details.OrderTotal.ToString();
                        idTextBox.Text = _details.OrderId.ToString();
                        datePicker.Text = _details.OrderDate.ToString();
                        if (_details.OrderName != null) customerTextbox.Text = _details.OrderName.ToString();
                        if (_details.OrderPhone != null) phoneTextBox.Text = _details.OrderPhone.ToString();
                        if (_details.OrderAddress != null) addressTextBox.Text = _details.OrderAddress.ToString();
                    }
                }
            });
        }

        private void _createColumnsCart()
        {
            Cart_DataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "ID",
                Binding = new Binding("ProductId"),
                IsReadOnly = true,
                MaxWidth = 80,
            });
            Cart_DataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "Name",
                Binding = new Binding("ProductName") { StringFormat = "MM/dd/yyyy" },
                IsReadOnly = true,
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
            });
            Cart_DataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "Price",
                IsReadOnly = true,
                Binding = new Binding("OrderdetailPrice"),
                MaxWidth = 100,
            });
            //Nguồn cập nhật khi giá trị thay đổi
            var binding = new Binding("OrderdetailQuantity");
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            Cart_DataGrid.Columns.Add(new DataGridTextColumn()
            {
                Header = "Quantity",
                IsReadOnly = true,
                Binding = binding,
                MaxWidth = 100,
            });
        }

        private void searchOrderTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (kindComboBox.SelectedIndex == 0)
                {
                    if (string.IsNullOrEmpty(searchOrderTextBox.Text)) Order_DataGrid.ItemsSource = _saleBUS.Orders;
                    else
                    if (_saleBUS.Orders != null) Order_DataGrid.ItemsSource = _saleBUS.Orders.Where(o =>
                        o.OrderId.ToString() == searchOrderTextBox.Text ||
                        string.IsNullOrEmpty(o.OrderName) ? false: o.OrderName.Contains(searchOrderTextBox.Text, StringComparison.OrdinalIgnoreCase) ||
                        o.OrderDate.Equals(searchOrderTextBox));
                }
                else
                {
                    if (string.IsNullOrEmpty(searchOrderTextBox.Text)) Order_DataGrid.ItemsSource = _importBUS.Orders;
                    else if (_importBUS.Orders != null) Order_DataGrid.ItemsSource = _importBUS.Orders.Where(o =>
                        o.OrderId.ToString() == searchOrderTextBox.Text ||
                        string.IsNullOrEmpty(o.OrderName) ? false : o.OrderName.Contains(searchOrderTextBox.Text, StringComparison.OrdinalIgnoreCase) ||
                        o.OrderDate.Equals(searchOrderTextBox));

                }
            }
        }
        private void searchProductTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(searchProductTextBox.Text)) Product_DataGrid.ItemsSource = _product_BUS.Products;
                else if (_product_BUS.Products != null) Product_DataGrid.ItemsSource = _product_BUS.Products.Where(p => p.ProductName == null? false:p.ProductName.Contains(searchProductTextBox.Text, StringComparison.OrdinalIgnoreCase) || p.ProductId.ToString() == searchProductTextBox.Text);
            }
        }


        private void newOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                Cart_DataGrid.Columns.Clear();
                Cart_DataGrid.IsReadOnly = false;
                Cart_DataGrid.CanUserDeleteRows = true;
                _createColumnsCart();
                Cart_DataGrid.Columns[3].IsReadOnly = false;
                _order = new OrderDTO();
                if (kindComboBox.SelectedIndex == 0) _order.OrderId =  EShopDbContext.Instance.SaleOrders.Select(x => x.OrderId).DefaultIfEmpty().Max() + 1;
                else if (kindComboBox.SelectedIndex == 1) _order.OrderId = EShopDbContext.Instance.ImportOrders.Select(x => x.OrderId).DefaultIfEmpty().Max() + 1;
                idTextBox.Text = _order.OrderId.ToString();
                Cart_DataGrid.ItemsSource = _order.OrderDetails;
                Total_lable.Text = "0";
                datePicker.Text = DateTime.Now.ToString("MM/dd/yyyy");
                customerTextbox.Text = "";
                phoneTextBox.Text = "";
                addressTextBox.Text = "";
                checkoutButton.Visibility = Visibility.Visible;
                cartaddButton.Visibility = Visibility.Visible;
                cartremoveButton.Visibility = Visibility.Visible;
                customerTextbox.IsReadOnly = false;
                datePicker.IsEnabled = true;
                phoneTextBox.IsReadOnly = false;
                addressTextBox.IsReadOnly = false;
            }));
        }
        private void _updateTotal()
        {
            if (_order != null) Total_lable.Text = (kindComboBox.SelectedIndex == 0)? _order.OrderDetails.Sum(item => item.OrderdetailQuantity * item.OrderdetailPrice).ToString() : _order.OrderDetails.Sum(item => item.OrderdetailQuantity * item.OrderdetailPrice).ToString() ;
        }

        private void Cart_DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            _updateTotal();
        }
        private void cartaddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                var _product = (Product_DTO)Product_DataGrid.SelectedItem;
                if (_product == null) return;
                if (_order != null) 
                { 
                    var item = _order.OrderDetails.FirstOrDefault(x => x.ProductId == _product.ProductId);
                    if (kindComboBox.SelectedIndex == 0 && _product.ProductQuantity <= 0) return;
                    if (item == null)
                    {
                        var _detail = new OrderDetailDTO();
                        _detail.ProductId = _product.ProductId;
                        _detail.OrderdetailPrice = _product.ProductSellprice;
                        _detail.OrderdetailQuantity = 1;
                        _detail.ProductName = _product.ProductName;
                        _detail.OrderId = int.Parse(idTextBox.Text);
                        _order.OrderDetails.Add(_detail);
                    }
                    else
                    {
                        if (kindComboBox.SelectedIndex == 0 &&  item.OrderdetailQuantity > _product.ProductQuantity) return;
                        item.OrderdetailQuantity++;
                        Cart_DataGrid.Items.Refresh();
                    }
                    _updateTotal();
                }
            }));
        }
        private void cartremoveButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var _product = (OrderDetailDTO)Cart_DataGrid.SelectedItem;
                if (_product != null && _order != null)
                {
                    var item = _order.OrderDetails.FirstOrDefault(x => x.ProductId == _product.ProductId);
                    if (item != null) _order.OrderDetails.Remove(item);
                    _updateTotal();
                }
            }));

        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var _selected = (OrderDTO)Order_DataGrid.SelectedItem;
                if (Cart_DataGrid != null && _selected != null && kindComboBox != null)
                {
                    Cart_DataGrid.Columns.Clear();
                    Cart_DataGrid.IsReadOnly = false;
                    Cart_DataGrid.CanUserDeleteRows = true;
                    _createColumnsCart();
                    Cart_DataGrid.Columns[3].IsReadOnly = false;
                    checkoutButton.Visibility = Visibility.Visible;
                    cartaddButton.Visibility = Visibility.Visible;
                    cartremoveButton.Visibility = Visibility.Visible;
                    customerTextbox.IsReadOnly = false;
                    datePicker.IsEnabled = true;
                    phoneTextBox.IsReadOnly = false;
                    addressTextBox.IsReadOnly = false;
                    if (kindComboBox.SelectedIndex == 0 && _saleBUS.Orders != null)
                    {
                        _order = _saleBUS.Orders.First(s => s.OrderId == _selected.OrderId);
                    }
                    else if (kindComboBox.SelectedIndex == 1 && _importBUS.Orders != null)
                    {
                        _order = _importBUS.Orders.First(s => s.OrderId == _selected.OrderId);
                    }
                    if (_order != null)
                    {
                        Cart_DataGrid.ItemsSource = _order.OrderDetails;
                        Total_lable.Text = _order.OrderTotal.ToString();
                        idTextBox.Text = _order.OrderId.ToString();
                        datePicker.Text = _order.OrderDate.ToString();
                        if (_order.OrderName != null) customerTextbox.Text = _order.OrderName.ToString();
                        if (_order.OrderPhone != null) phoneTextBox.Text = _order.OrderPhone.ToString();
                        if (_order.OrderAddress != null) addressTextBox.Text = _order.OrderAddress.ToString();
                    }
                }
            }));
        }
        private void kindComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Order_DataGrid == null) return;
            if (Product_DataGrid != null)
            {
                if (kindComboBox.SelectedIndex == 0) Product_DataGrid.Columns[2] = new DataGridTextColumn()
                {
                    Header = "Sale Price",
                    IsReadOnly = true,
                    Binding = new Binding("ProductSellprice"),
                    MaxWidth = 100,
                };
                else Product_DataGrid.Columns[2] = new DataGridTextColumn()
                {
                    Header = "Import Price",
                    IsReadOnly = true,
                    Binding = new Binding("ProductImportprice"),
                    MaxWidth = 100,
                };

            }
            _reloadOrderData();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var __order = Order_DataGrid.SelectedItem as OrderDTO;
            if (__order == null) return;
            if (_order !=null) if (_order.OrderDetails != null)
            {
                if (MyMessageBox.Show("Do you want to delete this order?", "Comfirm"))
                {
                    
                    if (kindComboBox.SelectedIndex == 0)
                    {
                        if (__order != null) { _saleBUS.Remove(__order); }
                    }
                    else
                    {
                        if (__order != null) { _importBUS.Remove(__order); }
                    }
                }
            }
        }

        private void checkoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyMessageBox.Show("Do you want to checkout this order?", "Comfirm"))
            {
                if (_order != null) if (_order.OrderDetails != null)
                {
                    if (_order.OrderDetails.Count == 0) { MyMessageBox.Show("Cart is empty!", "Warning"); return; }
                    _order.OrderDate = DateTime.Parse(datePicker.Text);
                    if (string.IsNullOrEmpty(_order.OrderDate.ToString())) { MyMessageBox.Show("Date is empty!", "Warning"); return; }
                    _order.OrderName = customerTextbox.Text;
                    _order.OrderPhone = phoneTextBox.Text;
                    _order.OrderAddress = addressTextBox.Text;
                    _order.OrderTotal = decimal.Parse(Total_lable.Text);
                    if (kindComboBox.SelectedIndex == 0) _saleBUS.Write(_order); else _importBUS.Write(_order);
                }
                cartaddButton.Visibility = Visibility.Collapsed;
                cartremoveButton.Visibility = Visibility.Collapsed;
                checkoutButton.Visibility = Visibility.Collapsed;
                datePicker.IsEnabled = false;
                customerTextbox.IsReadOnly = true;
                phoneTextBox.IsReadOnly = true;
                addressTextBox.IsReadOnly = true;

            }
        }
        /// <summary>
        /// Kiểm tra chỉ nhận giá trị số
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PageSizeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]*$");
            e.Handled = regex.IsMatch(e.Text);
        }

    }
}
