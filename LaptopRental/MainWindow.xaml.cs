using LaptopRental.Data;
using LaptopRental.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LaptopRental
{

    public partial class MainWindow : Window
    {
        private readonly ApplicationDbContext context = new();
        public MainWindow()
        {
            InitializeComponent();
            SeedDatabase();

            BrandSelection.ItemsSource = context.Brands.ToList();
            // Select "Any" by default
            BrandSelection.SelectedIndex = 0;
            //AvailableLaptops.ItemsSource = context.Laptops.ToList();

            // Set the laptops data grid source
            LaptopsData.ItemsSource = context.Laptops.ToList();

            // Set the bookings data grid source
            BookingsData.ItemsSource = context.Bookings.ToList();
        }

        private void SeedDatabase()
        {
            // Add initial brand data if none exists
            if (!context.Brands.Any())
            {
                List<Brand> brands = [
                    new Brand("Any"),
                    new Brand("Microsoft"),
                    new Brand("Dell"),
                    new Brand("Apple"),

                ];
                context.Brands.AddRange(brands);
                _ = context.SaveChanges();
            }

            // Add initial laptop data if none exists
            if (!context.Laptops.Any())
            {
                List<Laptop> laptops = [
                    new Laptop(2, "Surface Pro", "i7 16GB 4K"),
                    new Laptop(3, "XPS 15", "i7 32GB RTX 4070"),
                    new Laptop(2, "Surface Duo", "i5 8GB Touchscreen"),
                    new Laptop(3, "Latitude", "AMD Ryzen 8GB"),
                    new Laptop(4, "Macbook Pro", "M3 Max 32GB"),
                    new Laptop(4, "Macbook Air", "M2 8GB")
                ];
                context.Laptops.AddRange(laptops);
                _ = context.SaveChanges();
            }
        }

        // Handle Booking submission
        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if laptop is selected
            if (AvailableLaptops.SelectedItem == null)
            {
                _ = MessageBox.Show("Please select a laptop to book");
                return;
            }

            // Get selected dates and laptop
            DateOnly[] selectedDates = { GetDateFromPicker(StartDatePicker), GetDateFromPicker(EndDatePicker) };
            Laptop laptop = (Laptop)AvailableLaptops.SelectedItem;

            // Check if laptop is available for selected dates
            if (!CheckLaptopAvailability(laptop.Id, startDate: selectedDates[0], endDate: selectedDates[1]))
            {
                _ = MessageBox.Show("Laptop not available for selected dates");
                return;
            }

            AddNewBooking(startDate: selectedDates[0], endDate: selectedDates[1], laptop);
            MessageBox.Show("Booking successful!");
            ResetFormInputs();

        }

        private void ResetFormInputs()
        {
            BrandSelection.SelectedIndex = 0;
            StartDatePicker.SelectedDate = null;
            EndDatePicker.SelectedDate = null;
            AvailableLaptops.ItemsSource = null;
        }

        private void AddNewBooking(DateOnly startDate, DateOnly endDate, Laptop laptop)
        {
            // Create booking
            Booking booking = new()
            {
                StartDate = startDate,
                EndDate = endDate,
                Laptop = laptop
            };
            _ = context.Bookings.Add(booking);
            _ = context.SaveChanges();
            RefreshBookingsDataGrid();
        }

        private void RefreshBookingsDataGrid()
        {
            BookingsData.ItemsSource = context.Bookings.ToList();
        }

        // Checks if laptops available at selected dates
        public bool CheckLaptopAvailability(int laptopId, DateOnly startDate, DateOnly endDate)
        {
            IQueryable<Booking> bookings = context.Bookings.Where(b => b.LaptopId == laptopId);
            if (bookings.Any())
            {
                foreach (Booking? booking in bookings)
                {
                    if ((endDate <= booking.StartDate) || (startDate >= booking.EndDate))
                    {
                        return true; // Laptop available
                    }
                    return false; // Laptop not available
                }
            }
            return true;

        }

        // Clear available laptops list
        public void ResetAvailableLaptops(object sender, RoutedEventArgs e)
        {
            AvailableLaptops.ItemsSource = null;
        }

        public bool ValidateSearchForm()
        {
            // Check if dates are selected
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                _ = MessageBox.Show("Please select both start and end dates");
                return false;
            }

            // Check if start date is in the past
            if (StartDatePicker.SelectedDate < DateTime.Now)
            {
                _ = MessageBox.Show("Start date cannot be in the past");
                return false;
            }

            // Check if start date is after end date
            if (StartDatePicker.SelectedDate > EndDatePicker.SelectedDate)
            {
                _ = MessageBox.Show("Start date cannot be after end date");
                return false;
            }
            return true;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if form is valid
            if (!ValidateSearchForm()) { return; }

            // Get the laptops of the selected brand or all laptops
            object selectedBrand = BrandSelection.SelectedItem;
            List<Laptop> laptops = selectedBrand == context.Brands.FirstOrDefault(b => b.Name == "Any")
                ? context.Laptops.ToList()
                : context.Laptops.Where(l => l.Brand == selectedBrand).ToList();

            // Get the bookings only where the laptop is of the selected branc
            List<Booking> bookings = context.Bookings.Where(b => b.Laptop.Brand == selectedBrand).ToList();

            // List to store available laptops
            List<Laptop> availableLaptops = laptops;

            // For each booking, check if the dates clash with selected search dates for the laptop
            DateOnly[] selectedDates = { GetDateFromPicker(StartDatePicker), GetDateFromPicker(EndDatePicker) };
            foreach (Booking? booking in bookings)
            {
                // If laptop is not available, remove from available laptops list
                if (!CheckLaptopAvailability(booking.LaptopId, startDate: selectedDates[0], endDate: selectedDates[1]))
                {
                    _ = availableLaptops.Remove(booking.Laptop);
                }
            }

            // Set the available laptops data source
            AvailableLaptops.ItemsSource = availableLaptops;

        }

        // Gets date from date picker and returns DateOnly
        private DateOnly GetDateFromPicker(DatePicker datePicker)
        {
            return DateOnly.FromDateTime(datePicker.SelectedDate.Value.Date);
        }

        // Handle laptop selection details display update
        private void AvailableLaptops_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if laptop is selected - required for when ResetFormInputs is called
            if (!(AvailableLaptops.ItemsSource == null))
            {
                Uri uri = new($"Images/{((Laptop)AvailableLaptops.SelectedItem).Name}.jpg", UriKind.Relative);
                SelectedLaptopImage.Source = new BitmapImage(uri);
                StringBuilder laptopDetails = new StringBuilder();
                laptopDetails.AppendLine($"Brand: {((Laptop)AvailableLaptops.SelectedItem).Brand}");
                laptopDetails.AppendLine($"Name: {((Laptop)AvailableLaptops.SelectedItem).Name}");
                laptopDetails.AppendLine($"Specs: {((Laptop)AvailableLaptops.SelectedItem).Description}");
                laptopDetails.AppendLine($"Start Date: {GetDateFromPicker(StartDatePicker)}");
                laptopDetails.AppendLine($"End Date: {GetDateFromPicker(EndDatePicker)}");
                SelectedLaptopDetails.Text = laptopDetails.ToString();
            }
            else
            {
                SelectedLaptopImage.Source = null;
                SelectedLaptopDetails.Text = "";
            }
        }

        // Handle booking deletion
        private void DeleteBookingBtn_Click(object sender, RoutedEventArgs e)
        {
            var booking = BookingsData.SelectedItem;
            if (booking == null)
            {
                _ = MessageBox.Show("Please select a booking to delete");
                return;
            }
            context.Bookings.Remove((Booking)booking);
            context.SaveChanges();
            RefreshBookingsDataGrid();
        }
    }

}