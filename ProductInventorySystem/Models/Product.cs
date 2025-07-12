using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductInventorySystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName ="decimal(10,2)")]
        public decimal Price { get; set; }

        [MaxLength(50)]
        public string Category { get; set; }


    }
}
