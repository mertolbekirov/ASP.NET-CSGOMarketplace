namespace CSGOMarketplace.Services.Items.Models
{
    public class ItemServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public double? Float { get; init; }

        public string Condition { get; init; }

        public string ImageUrl { get; init; }
        public string InspectUrl { get; init; }

        public decimal Price { get; init; }
    }
}
