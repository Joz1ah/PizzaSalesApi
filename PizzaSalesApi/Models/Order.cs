using CsvHelper.Configuration.Attributes;

namespace PizzaSalesApi.Models
{
    public class Order
    {
        [Name("order_id")]
        public int OrderId { get; set; }

        [Name("date")]
        public DateTime Date { get; set; }

        [Name("time")]
        public TimeSpan Time { get; set; }
    }
}
