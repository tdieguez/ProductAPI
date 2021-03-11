namespace MyStore.OpenApi.V1.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public long? CategoryId { get; set; }
    }
}