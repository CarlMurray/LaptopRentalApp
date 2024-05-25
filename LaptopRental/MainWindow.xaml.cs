using LaptopRental.Data;
using LaptopRental.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            //AvailableLaptops.ItemsSource = context.Laptops.ToList();

            // Set the laptops data grid source
            LaptopsData.ItemsSource = context.Laptops.ToList();

            // Set the bookings data grid source
            BookingsData.ItemsSource = context.Bookings.ToList();
        }

        //private void BrandSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    // Check if "Any" selected
        //    if (AvailableLaptops.SelectedIndex == 0)
        //    {
        //        AvailableLaptops.ItemsSource = context.Laptops.ToList();

        //    }
        //    else AvailableLaptops.ItemsSource = context.Laptops.Where(l => l.Brand == BrandSelection.SelectedItem).ToList();

        //}

        // Handle Booking submission
        private void BookButton_Click(object sender, RoutedEventArgs e)
        {


            // Check if laptop is selected
            if (AvailableLaptops.SelectedItem == null)
            {
                MessageBox.Show("Please select a laptop to book");
                return;
            }

            DateOnly startDate = DateOnly.FromDateTime(StartDatePicker.SelectedDate.Value.Date);
            DateOnly endDate = DateOnly.FromDateTime(EndDatePicker.SelectedDate.Value.Date);
            Laptop laptop = (Laptop)AvailableLaptops.SelectedItem;

            if(!CheckLaptopAvailability(laptop.Id, startDate, endDate) )
            {
                MessageBox.Show("Laptop not available for selected dates");
                return;
            }


            Booking booking = new Booking
            {
                StartDate = startDate,
                EndDate = endDate,
                Laptop = laptop
            };

            context.Bookings.Add(booking);
            context.SaveChanges();
            BookingsData.ItemsSource = context.Bookings.ToList();


        }

        // Checks if laptops available at selected dates
        public bool CheckLaptopAvailability(int laptopId, DateOnly startDate, DateOnly endDate)
        {
            var bookings = context.Bookings.Where(b => b.LaptopId == laptopId);
            if (bookings.Any())
            {
                foreach (var booking in bookings)
                {
                    if ((endDate < booking.StartDate) || (startDate > booking.EndDate))
                    {
                        return true; // Laptop available
                    }
                    return false; // Laptop not available
                }
            }
            return true;

        }

        public void ResetAvailableLaptops(object sender, RoutedEventArgs e)
        {
            AvailableLaptops.ItemsSource = null;
        }

        public bool ValidateSearchForm()
        {
            // Check if dates are selected
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please select both start and end dates");
                return false;
            }

            // Check if start date is in the past
            if (StartDatePicker.SelectedDate < DateTime.Now)
            {
                MessageBox.Show("Start date cannot be in the past");
                return false;
            }

            // Check if start date is after end date
            if (StartDatePicker.SelectedDate > EndDatePicker.SelectedDate)
            {
                MessageBox.Show("Start date cannot be after end date");
                return false;
            }
            return true;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateSearchForm()) { return;}

            var selectedBrand = BrandSelection.SelectedItem;
            List<Laptop> laptops;

            // Check if "Any" selected
            if (selectedBrand == context.Brands.FirstOrDefault(b => b.Name == "Any"))
            {
                laptops = context.Laptops.ToList();
            }
            // If not, get laptops of the selected brand
            else laptops = context.Laptops.Where(l => l.Brand == selectedBrand).ToList();

            // Get the bookings only where the laptop is of the selected branc
            var bookings = context.Bookings.Where(b => b.Laptop.Brand == selectedBrand).ToList();

            //
            List<Laptop> availableLaptops = laptops;

            // For each booking, check if the dates clash with selected search dates for the laptop
            DateOnly startDate = DateOnly.FromDateTime(StartDatePicker.SelectedDate.Value.Date);
            DateOnly endDate = DateOnly.FromDateTime(EndDatePicker.SelectedDate.Value.Date);
            foreach (var booking in bookings)
            {
                // If laptop is not available, remove from available laptops list
                if(!CheckLaptopAvailability(booking.LaptopId, startDate, endDate))
                {
                    availableLaptops.Remove(booking.Laptop);
                }
            }

            // Set the available laptops data source
            AvailableLaptops.ItemsSource = availableLaptops;

        }

    }
}