namespace MyStore.OpenApi.V1.ViewModels
{
    public class ProductViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}