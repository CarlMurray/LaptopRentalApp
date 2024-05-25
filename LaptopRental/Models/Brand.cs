namespace LaptopRental.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";

        public Brand(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
