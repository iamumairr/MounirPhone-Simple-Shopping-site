namespace MounirPhone.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public decimal OrderPrice { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
