using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MounirPhone.Data;

namespace MounirPhone.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var productCarts = _context.Carts.Include(a=>a.Product).ToList();

            return View(productCarts);
        }
    }
}
