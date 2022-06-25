using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MounirPhone.Data;
using MounirPhone.Models;
using System.Diagnostics;

namespace MounirPhone.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutMe()
        {
            return View();
        }
        public IActionResult News()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ContactUs(ContactDetails contactDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactDetails); //insert statement
                _context.SaveChanges();

                ViewBag.Message = "Data submitted successfully.";

                return RedirectToAction(nameof(ContactUs));
            }
            return View();
        }
        public async Task<IActionResult> Product(string searchString)
        {
            var products = from m in _context.Products
                           select m; //linq query
            //select * from Products

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name!.Contains(searchString)); 
                //John
                //jo
            }

            return View(await products.ToListAsync());
        }

        public IActionResult AddToCart(int? Id) //Home/AddToCart/5
        {
            if (Id == null)
            {
                return NotFound();
            }
            var product = _context.Products.FirstOrDefault(a => a.Id == Id);
            //select * from Products where Id = @Id
            if (product == null)
            {
                return NotFound();
            }

            Cart cart = new Cart
            {
                ProductId = product.Id,
                Username = _userManager.GetUserName(User),
                Quantity = 1,
                UnitPrice = product.Price, //10x1
                SubTotal = product.Price //price X quantity
            };

            _context.Add(cart);
            _context.SaveChanges();

            return RedirectToAction(nameof(ShoppingCart));
        }
        public IActionResult ShoppingCart()
        {
            var carts = _context.Carts.Include(a=>a.Product).ToList();
            return View(carts);
        }

        public IActionResult EditCart(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var cart = _context.Carts.Find(Id);
            if (cart == null)
            {
                return NotFound();
            }
            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCart(int Id, Cart cart)
        {
            if (Id != cart.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (cart.Quantity > 1)
                {
                    var objFromDb = _context.Carts.FirstOrDefault(a => a.Id == Id);

                    objFromDb.Quantity = cart.Quantity;
                    objFromDb.SubTotal = objFromDb.UnitPrice * cart.Quantity;

                    _context.Update(objFromDb);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(ShoppingCart));
            }
            return View(cart);
        }

        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var cart = _context.Carts.Find(Id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            _context.SaveChanges();
            return RedirectToAction(nameof(ShoppingCart));
        }
        public IActionResult CheckOut()
        {
            Order order = new Order();
            order.OrderPrice = (decimal)_context.Carts.Where(a => a.Username == _userManager.GetUserName(User)).Select(a => a.Quantity * a.UnitPrice).Sum();

            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                order.EntryDate = DateTime.Now;
                order.Username = _userManager.GetUserName(User);

                _context.Add(order);
                _context.SaveChanges();

                //delete cart for username
                var carts = _context.Carts.Where(a => a.Username == _userManager.GetUserName(User)).ToList();
                _context.Carts.RemoveRange(carts);
                _context.SaveChanges();

                return RedirectToAction(nameof(SuccessMessage));
            }
            return View(order);
        }

        public IActionResult SuccessMessage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}