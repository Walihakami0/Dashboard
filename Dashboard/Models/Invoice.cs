using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        public int CostumerId { get; set; }

        public int ProductId { get; set; }
        public decimal Price { get; set; }

        public int QTY { get; set; }

        public float Tax { get; set; }
        public decimal Discount { get; set; }

        public decimal Total { get; set; }
    }
}
