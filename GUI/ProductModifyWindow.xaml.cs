using _21880263.DTO;
using _21880263.GUI;
using BespokeFusion;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace _21880263
{
    /// <summary>
    /// Interaction logic for ProductDetails_Window.xaml
    /// </summary>
    public partial class ProductDetails_Window : Window
    {
        Product_DTO _product;
        Product_DTO product;
        public ProductDetails_Window(Product_DTO product, BindingList<Category_DTO> category, BindingList<Brand_DTO> brand)
        {
            InitializeComponent();
            this.product = product;
            this._product = (Product_DTO)product.Clone();
            this.DataContext = _product;
            categoryCombobox.ItemsSource = category;
            brandCombobox.ItemsSource = brand;
            var __category = category.FirstOrDefault(s => s.CategoryId == product.CategoryId);
            if (__category != null) categoryCombobox.SelectedIndex = category.IndexOf(__category);
            var __brand = brand.FirstOrDefault(s => s.BrandId == product.BrandId);
            if (__brand != null) brandCombobox.SelectedIndex = brand.IndexOf(__brand);
        }

        private void OK_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(nameTextbox.Text))
            {
                if (!MyMessageBox.Show("Please input Name!", "Error")) { this.Close(); return; }
                return;
            }
            if (categoryCombobox.SelectedIndex < 0)
            {
                ;
                if (!MyMessageBox.Show("Please select Category", "Error")) { this.Close(); return; }
                return;
            }
            if (categoryCombobox.SelectedIndex < 0)
            {
                
                if (!MyMessageBox.Show("Please select Brand", "Error")) { this.Close(); return; }
                return;
            }
            decimal temp;

            product.ProductName = nameTextbox.Text;
            product.CategoryId = ((Category_DTO)categoryCombobox.SelectedItem).CategoryId;
            product.BrandId = ((Brand_DTO)brandCombobox.SelectedItem).BrandId;
            product.ProductImportprice = decimal.TryParse(importPriceTextBox.Text, out temp)? temp: 0;
            product.ProductSellprice = decimal.TryParse(sellPriceTextBox.Text, out temp) ? temp : 0;
            product.ProductDescription = descriptionTextBox.Text;
            product.ProductImage = _product.ProductImage;
            DialogResult = true;
            this.Close();
        }

        private void loadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = "jpg files (*.jpg)|*.jpg",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFile.ShowDialog() == true)
            {
                _product.ProductImage = new BitmapImage();
                _product.ProductImage.BeginInit();
                _product.ProductImage.UriSource = new Uri(openFile.FileName);
                _product.ProductImage.CacheOption = BitmapCacheOption.OnLoad;
                _product.ProductImage.EndInit();
                ProductImage.Source = _product.ProductImage;
            };
        }


    }
}
