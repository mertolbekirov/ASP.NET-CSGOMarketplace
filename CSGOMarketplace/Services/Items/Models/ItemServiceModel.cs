namespace CSGOMarketplace.Services.Items.Models
{
    public class ItemServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public double Float { get; init; }

        public string ConditionName { get; init; }

        public string ImageUrl { get; set; }
        public string InspectUrl { get; set; }

        public string OwnerId { get; set; }

        public decimal Price { get; set; }
    }
}
