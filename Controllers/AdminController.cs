using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MounirPhone.Data;

namespace MounirPhone.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ContactDetails()
        {
            var contacts = _context.ContactDetails.ToList();
            return View(contacts);
        }
    }
}
