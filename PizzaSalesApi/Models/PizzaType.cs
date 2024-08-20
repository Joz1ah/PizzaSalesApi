using CsvHelper.Configuration.Attributes;

namespace PizzaSalesApi.Models
{
    public class PizzaType
    {
        [Name("pizza_type_id")]
        public required string PizzaTypeId { get; set; }

        [Name("name")]
        public required string Name { get; set; }

        [Name("category")]
        public required string Category { get; set; }

        [Name("ingredients")]
        public  required string Ingredients { get; set; }
    }
}
