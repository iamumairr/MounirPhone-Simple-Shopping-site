using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MounirPhone.Data;
using MounirPhone.Models;

namespace MounirPhone.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _WebHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }
      
        // GET: Products/Create/
        public IActionResult Create()
        {
            return View();
        }
        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _WebHostEnvironment.WebRootPath;
                string path = @"\Uploads\";

                string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);

                product.ProductImage = fileName = fileName + DateTime.Now.ToString("yyyymmssfff") + extension;

                string pathforSave = Path.Combine(rootPath + path + fileName);
                using (var fileStream = new FileStream(pathforSave, FileMode.Create))
                {
                    product.ImageFile.CopyTo(fileStream);
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var objfromDb = _context.Products.AsNoTracking().FirstOrDefault(a => a.Id == id);

                    if (product.ImageFile != null)
                    {
                        string rootPath = _WebHostEnvironment.WebRootPath;
                        string path = @"\Uploads\";

                        string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                        string extension = Path.GetExtension(product.ImageFile.FileName);

                        product.ProductImage = fileName = fileName + DateTime.Now.ToString("yyyymmssfff") + extension;

                        string pathforSave = Path.Combine(rootPath + path + fileName);

                        var oldImage = Path.Combine(rootPath + path, objfromDb.ProductImage);
                        if (System.IO.File.Exists(oldImage))
                        {
                            System.IO.File.Delete(oldImage);
                        }

                        using (var fileStream = new FileStream(pathforSave, FileMode.Create))
                        {
                            product.ImageFile.CopyTo(fileStream);
                        }
                    }
                    else
                    {
                        product.ProductImage = objfromDb.ProductImage;
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            string path = @"\Uploads\";
            string uploadPath = _WebHostEnvironment.WebRootPath + path;

            var oldPicture = Path.Combine(uploadPath + product.ProductImage);

            if (System.IO.File.Exists(oldPicture))
            {
                System.IO.File.Delete(oldPicture);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
