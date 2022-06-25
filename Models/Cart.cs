namespace MounirPhone.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Username { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? SubTotal { get; set; }
        public virtual Product Product { get; set; }
    }
}
