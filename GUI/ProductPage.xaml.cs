using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using _21880263.BUS;
using _21880263.DAO;
using _21880263.DTO;
using _21880263.GUI;

namespace _21880263
{

    /// <summary>
    /// Interaction logic for ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        Product_BUS? Product_BUS { get; set; }
        
        string? keyword { get; set; }
        int _currentPage { get; set; }
        int _pageSize { get; set; } = 10;
        public ProductPage(MainWindow mainWindow)
        {
            InitializeComponent();
            if (mainWindow != null) mainWindow.SearchUpdating += MainWindow_SearchUpdating;
        }

        private void MainWindow_SearchUpdating(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    this.Product_BUS = new Product_BUS();
                    if (Product_BUS.Products!= null) ProductListView.ItemsSource = Product_BUS.Products.Where(p => p.ProductName ==null ? false: p.ProductName.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
                });
            }
            else _reloadProduct(_currentPage, _pageSize);
            //Product_BUS.Search(keyword);
        }

        /// <summary>
        /// Tim parent cua muc da chon
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T? FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            T parent = (T)parentObject;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }
        public void SetReadOnly(DependencyObject parent)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is TextBox)
                {
                    ((TextBox)child).IsReadOnly = true;
                }
                else if (child is ComboBox)
                {
                    ((ComboBox)child).IsEnabled = false;
                } else if (child is Button)
                {
                    ((Button)child).Visibility = Visibility.Hidden;
                }

                // Gọi đệ quy cho các control con
                SetReadOnly(child);
            }
        }

        private void Add_ButtonClick(object sender, RoutedEventArgs e)
        {
            Product_DTO product = new Product_DTO();
            product.ProductId = EShopDbContext.Instance.Products.Select(x => x.ProductId).DefaultIfEmpty().Max()+1;
            if (Product_BUS != null) if (Product_BUS.Categories != null && Product_BUS.Brands != null)
            {
                var screen = new ProductDetails_Window(product, Product_BUS.Categories, Product_BUS.Brands);
                if (screen.ShowDialog() == true) Product_BUS.Add(product);
            }
        }
        private void Edit_ButtonClick(object sender, RoutedEventArgs e)
        {
            var product = (Product_DTO)ProductListView.SelectedItem;
            if (product == null) { return; }
            if (Product_BUS != null) if (Product_BUS.Categories != null && Product_BUS.Brands != null)
                {
                    var screen = new ProductDetails_Window(product, Product_BUS.Categories, Product_BUS.Brands);
                    if (screen.ShowDialog() == true) Product_BUS.Edit(product);
                }
        }
        private void Remove_ButtonClick(object sender, RoutedEventArgs e)
        {
            var product = (Product_DTO)ProductListView.SelectedItem;
            if (product == null) { return; }
            if (MyMessageBox.Show("Do you really wanto remove this item?", "Confirm")) if (Product_BUS != null) Product_BUS.Remove(product);
        }

        private void Details_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (Product_BUS != null) if (Product_BUS.Categories != null && Product_BUS.Brands != null)
                {
                    var screen = new ProductDetails_Window((Product_DTO)ProductListView.SelectedItem, Product_BUS.Categories, Product_BUS.Brands);
                    SetReadOnly(screen.mainPanel);
                    screen.ShowDialog();
                }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Bắt đầu animation của progress bar
            ProgressBarLoad.IsIndeterminate = true;
            _currentPage = 1;
            // Thực hiện công việc bất đồng bộ
            await _reloadProduct(_currentPage, _pageSize);

            // Dừng animation của progress bar
            ProgressBarLoad.IsIndeterminate = false;
        }
        private Task _reloadProduct(int pageNumber = 0, int pageSize = 0)
        {
            this.Product_BUS = new Product_BUS(pageNumber, pageSize);
            // Thực hiện công việc bất đồng bộ tại đây
            this.Dispatcher.BeginInvoke(new Action(() => {
                if (Product_BUS != null) if (Product_BUS.Categories != null && Product_BUS.Brands != null)
                    {
                        ProductListView.ItemsSource = Product_BUS.Products;
                        CategoryComboBox.Items.Clear();
                        var combobox = new ComboBoxItem();
                        combobox.Content = "All";
                        combobox.Selected += new RoutedEventHandler(Category_Selected);
                        CategoryComboBox.Items.Add(combobox);
                        CategoryComboBox.SelectedIndex = 0;

                        BrandComboBox.Items.Clear();
                        combobox = new ComboBoxItem();
                        combobox.Content = "All";
                        combobox.Selected += new RoutedEventHandler(BrandComboBox_Selected);
                        BrandComboBox.Items.Add(combobox);
                        BrandComboBox.SelectedIndex = 0;

                        foreach (var item in Product_BUS.Categories)
                        {
                            combobox = new ComboBoxItem();
                            combobox.Tag = item;
                            combobox.Content = item.CategoryName;
                            combobox.Selected += new RoutedEventHandler(Category_Selected);
                            CategoryComboBox.Items.Add(combobox);
                        }
                        foreach (var item in Product_BUS.Brands)
                        {
                            combobox = new ComboBoxItem();
                            combobox.Tag = item;
                            combobox.Content = item.BrandName;
                            combobox.Selected += new RoutedEventHandler(BrandComboBox_Selected);
                            BrandComboBox.Items.Add(combobox);
                        }
                        if (pageComboBox.Items.Count != Product_BUS.TotalPage(_pageSize))
                        {
                            pageComboBox.Items.Clear();
                            for (int i = 1; i <= Product_BUS.TotalPage(_pageSize); i++)
                            {
                                pageComboBox.Items.Add(i + "/" + Product_BUS.TotalPage(_pageSize));
                            }
                        }
                        pageComboBox.SelectedIndex = _currentPage - 1 < 0 ? 0 : _currentPage - 1;
                        PageSizeTextBox.Text = _pageSize.ToString();
                    }
            }));
            return Task.CompletedTask;  
        }


        private void Category_Selected(object sender, RoutedEventArgs e)
        {
            if (Product_BUS != null) if (Product_BUS.Products != null)
                {
                    var item = (ComboBoxItem)sender;
                    if (ProductListView != null)
                    {
                        if (item.Tag == null)
                        {
                            ProductListView.ItemsSource = Product_BUS.Products;
                        }
                        else if (((ComboBoxItem)BrandComboBox.SelectedItem).Tag == null)
                        {
                            ProductListView.ItemsSource = Product_BUS.Products.Where(p => p.CategoryId == ((Category_DTO)item.Tag).CategoryId);
                        }
                        else
                        {
                            ProductListView.ItemsSource = Product_BUS.Products.Where(x => x.CategoryId == ((Category_DTO)item.Tag).CategoryId && x.BrandId == ((Brand_DTO)(((ComboBoxItem)BrandComboBox.SelectedItem).Tag)).BrandId);
                        }
                    }
                }
        }

        private void BrandComboBox_Selected(object sender, RoutedEventArgs e)
        {
            if (Product_BUS != null) if (Product_BUS.Products != null)
                {
                    var item = (ComboBoxItem)sender;
                    if (ProductListView != null)
                    {
                        if (item.Tag == null)
                        {
                            ProductListView.ItemsSource = Product_BUS.Products;
                        }
                        else if (((ComboBoxItem)CategoryComboBox.SelectedItem).Tag == null)
                        {
                            ProductListView.ItemsSource = Product_BUS.Products.Where(p => p.BrandId == ((Brand_DTO)item.Tag).BrandId);
                        }
                        else
                        {
                            ProductListView.ItemsSource = Product_BUS.Products.Where(x => x.CategoryId == ((Category_DTO)(((ComboBoxItem)CategoryComboBox.SelectedItem).Tag)).CategoryId && x.BrandId == ((Brand_DTO)item.Tag).BrandId);
                        }
                    }
                }
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var item = new Category_DTO();
            item.CategoryId = EShopDbContext.Instance.Categories.Select(x => x.CategoryId).DefaultIfEmpty().Max()+1;
            var screen = new CategoryWindow(item);
            if (screen.ShowDialog() == true)
            {
                if (Product_BUS != null) Product_BUS.AddCategory(item);
                _reloadProduct();
            }
        }

        private void EditCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (Category_DTO)((ComboBoxItem)CategoryComboBox.SelectedItem).Tag;
            if (item == null) return;
            var screen = new CategoryWindow(item);
            if (screen.ShowDialog() == true)
            {
                if (Product_BUS != null) Product_BUS.EditCategory(item);
                _reloadProduct();
            }
        }

        private void RemoveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (Category_DTO)((ComboBoxItem)CategoryComboBox.SelectedItem).Tag;
            if (item == null) return;
            bool check = EShopDbContext.Instance.Products.Select(s => s.CategoryId == item.CategoryId).FirstOrDefault();
            if (check) { MyMessageBox.Show("You can not delete this category. Please change or delete the products in this category", "Warning"); return; }
            if (MyMessageBox.Show($"Do you really want to delete {item.CategoryName} Category? ", "Confirm"))
            {
                if (Product_BUS != null) Product_BUS.RemoveCategory(item);
                _reloadProduct();
            }
        }

        private void AddBrandButton_Click(object sender, RoutedEventArgs e)
        {
            var item = new Brand_DTO();
            item.BrandId = EShopDbContext.Instance.Brands.Select(x => x.BrandId).DefaultIfEmpty().Max() + 1;
            var screen = new BrandWindow(item);
            if (screen.ShowDialog() == true)
            {
                if (Product_BUS != null) Product_BUS.AddBrand(item);
                _reloadProduct();
            }
        }

        private void EditBrandButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (Brand_DTO)((ComboBoxItem)BrandComboBox.SelectedItem).Tag;
            if (item == null) return;
            var screen = new BrandWindow(item);
            if (screen.ShowDialog() == true)
            {
                if (Product_BUS != null) Product_BUS.EditBrand(item);
                _reloadProduct();
            }
        }

        private void RemoveBrandButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (Brand_DTO)((ComboBoxItem)BrandComboBox.SelectedItem).Tag;
            if (item == null) return;
            bool check = EShopDbContext.Instance.Products.Select(s => s.BrandId == item.BrandId).FirstOrDefault();
            if (check) { MyMessageBox.Show("You can not delete this brand. Please change or delete the products in this brand", "Warning"); return; }
            if (MyMessageBox.Show($"Do you really want to delete {item.BrandName} Brand? ", "Confirm"))
            {
                if (Product_BUS != null) Product_BUS.RemoveBrand(item);
                _reloadProduct();
            }
        }

        private async void BeforePage_Click(object sender, RoutedEventArgs e)
        {
            // Bắt đầu animation của progress bar
            ProgressBarLoad.IsIndeterminate = true;

            if (_currentPage == 1) { ProgressBarLoad.IsIndeterminate = false; return; };
            _currentPage--;
            await _reloadProduct(_currentPage, _pageSize);

            // Dừng animation của progress bar
            ProgressBarLoad.IsIndeterminate = false;

        }

        private async void Nextpage_Click(object sender, RoutedEventArgs e)
        {
            // Bắt đầu animation của progress bar
            ProgressBarLoad.IsIndeterminate = true;

            if (Product_BUS != null) if (_currentPage == Product_BUS.TotalPage(_pageSize)) { ProgressBarLoad.IsIndeterminate = false; return; };
            _currentPage++;
            await _reloadProduct(_currentPage, _pageSize);

            // Dừng animation của progress bar
            ProgressBarLoad.IsIndeterminate = false;

        }

        private async void pageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pageComboBox.SelectedItem == null) return;
            // Bắt đầu animation của progress bar
            ProgressBarLoad.IsIndeterminate = true;
            
            _currentPage = pageComboBox.SelectedIndex+1;
            if (Product_BUS != null) if (_currentPage > 0 && _currentPage <= Product_BUS.TotalPage(_pageSize)) await _reloadProduct(_currentPage, _pageSize);

            // Dừng animation của progress bar
            ProgressBarLoad.IsIndeterminate = false;

        }

        private void PageSizeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async void PageSizeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _pageSize = int.Parse(PageSizeTextBox.Text);
                pageComboBox.SelectedItem = 1;
                await _reloadProduct(_currentPage, _pageSize);
            }

        }
    }
}
