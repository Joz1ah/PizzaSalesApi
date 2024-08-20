using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PizzaSalesApi.Models
{
    public class OrderDetail
    {
        [Key]
        [Name("order_details_id")]
        public int OrderDetailsId { get; set; } 

        [Name("order_id")]
        public int OrderId { get; set; }

        [Name("pizza_id")]
        public required string PizzaId { get; set; }

        [Name("quantity")]
        public int Quantity { get; set; }
    }
}
