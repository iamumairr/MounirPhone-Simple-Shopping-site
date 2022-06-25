using Microsoft.AspNetCore.Mvc;
using MounirPhone.Data;

namespace MounirPhone.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var orders = _context.Orders.ToList();

            return View(orders);
        }
        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var order = _context.Orders.Find(Id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
