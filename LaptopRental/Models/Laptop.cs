namespace LaptopRental.Models
{
    public class Laptop
    {
        public int Id { get; set; }
        public Brand Brand { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Laptop(int brandId, string name, string description)
        {
            BrandId = brandId;
            Name = name;
            Description = description;
        }
        public override string ToString()
        {
            return $"{Brand} {Name} {Description}";
        }
    }
}
