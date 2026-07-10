namespace MasterDetails.ApplicationServices.Dtos.ProductDto
{
    public class GetProductDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string DescriptionRecord { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
