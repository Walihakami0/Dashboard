using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Payments
    {
        [Key]
        public int Id { get; set; }
        public string EmailC { get; set; }
        public int MyProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public double Tax { get; set; }

        public int Invoice { get; set; }
        public DateTime DT { get; set; }
    }
}
