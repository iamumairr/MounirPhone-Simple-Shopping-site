using System.ComponentModel.DataAnnotations;

namespace MounirPhone.Models
{
    public class ContactDetails
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Name should be in between 5 and 50 letters", MinimumLength = 5)]
        public string Name { get; set; }
        [MinLength(10, ErrorMessage = "Subject can't be less than 10 letters")]
        [Required]
        public string Subject { get; set; }
        [MinLength(10, ErrorMessage = "Phone can't be less than 10 digits")]
        [MaxLength(10, ErrorMessage = "Phone can't be greater than 10 digits")]
        [Required]
        [RegularExpression("^[0-9]{10}$", ErrorMessage ="Phone should consist of 10 digits.")]
        public string Phone { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [MinLength(50,ErrorMessage ="Message can't be less than 50 letters")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Message { get; set; }
    }
}
