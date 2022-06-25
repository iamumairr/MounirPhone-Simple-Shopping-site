using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MounirPhone.Models
{
    public class Product
    {
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Display(Name="Product Image")]
        public string ProductImage { get; set; }
        [Display(Name = "Image")]
        [NotMapped]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }

        public ICollection<Cart> Carts { get; set; }
    }
}
