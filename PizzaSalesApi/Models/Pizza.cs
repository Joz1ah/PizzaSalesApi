using CsvHelper.Configuration.Attributes;

namespace PizzaSalesApi.Models
{
    public class Pizza
    {
        [Name("pizza_id")]
        public required string PizzaId { get; set; }
        [Name("pizza_type_id")]
        public required string PizzaTypeId { get; set; }
        [Name("size")]
        public required string Size { get; set; }
        [Name("price")]
        public decimal Price { get; set; }
    }
}
