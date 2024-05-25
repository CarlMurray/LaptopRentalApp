namespace LaptopRental.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int LaptopId { get; set; }
        public required Laptop Laptop { get; set; }
    }
}
