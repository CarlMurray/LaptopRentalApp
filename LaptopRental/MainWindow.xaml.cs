using LaptopRental.Data;
using LaptopRental.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LaptopRental
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public MainWindow()
        {
            InitializeComponent();

            // Add initial brand data if none exists
            if (!context.Brands.Any())
            {
                List<Brand> brands = new List<Brand> {
                    new Brand("Any"),
                    new Brand("Microsoft"),
                    new Brand("Dell"),
                    new Brand("Apple"),

                };
                context.Brands.AddRange(brands);
                context.SaveChanges();
            }

            // Add initial laptop data if none exists
            if (!context.Laptops.Any())
            {
                List<Laptop> laptops = new List<Laptop> {
                    new Laptop(2, "Surface Pro", "i7 16GB 4K"),
                    new Laptop(3, "XPS 15", "i7 32GB RTX 4070"),
                    new Laptop(2, "Surface Duo", "i5 8GB Touchscreen"),
                    new Laptop(3, "Latitude", "AMD Ryzen 8GB"),
                    new Laptop(4, "Macbook Pro", "M3 Max 32GB"),
                    new Laptop(4, "Macbook Air", "M2 8GB")
                };
                context.Laptops.AddRange(laptops);
                context.SaveChanges();
            }

            BrandSelection.ItemsSource = context.Brands.ToList();
            // Select "Any" by default
            BrandSelection.SelectedIndex = 0;
            AvailableLaptops.ItemsSource = context.Laptops.ToList();
        }

        private void BrandSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if "Any" selected
            if (AvailableLaptops.SelectedIndex == 0)
            {
                AvailableLaptops.ItemsSource = context.Laptops.ToList();

            }
            else AvailableLaptops.ItemsSource = context.Laptops.Where(l => l.Brand == BrandSelection.SelectedItem).ToList();

        }
    }
}