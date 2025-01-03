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

namespace _21880263
{
    /// <summary>
    /// Interaction logic for CategoryBrandWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {
        Category_DTO _category;
        public CategoryWindow(Category_DTO Category)
        {
            InitializeComponent();
            _category = Category;
            this.DataContext = Category.Clone();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            _category.CategoryName = nameTextBox.Text;
            _category.CategoryDescription = descriptionTextBox.Text;
            DialogResult = true;
            this.Close();
        }
    }
}
