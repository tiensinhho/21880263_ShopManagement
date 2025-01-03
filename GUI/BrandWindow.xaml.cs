using _21880263.DAO;
using _21880263.DTO;
using System;
using System.Collections.Generic;
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

namespace _21880263.GUI
{
    /// <summary>
    /// Interaction logic for BrandWindow.xaml
    /// </summary>
    /// 
    public partial class BrandWindow : Window
    {
        Brand_DTO _brand;
        public BrandWindow(Brand_DTO brand_DTO)
        {
            InitializeComponent();
            _brand = brand_DTO;
            this.DataContext = brand_DTO.Clone();
        }
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            _brand.BrandName = nameTextBox.Text;
            _brand.BrandDescription = descriptionTextBox.Text;
            DialogResult = true;
            this.Close();
        }

    }
}
