using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KickScooterSharing.Data;
using KickScooterSharing.Models;
using Microsoft.AspNetCore.Authorization;
using KickScooterSharing.ViewModels;

namespace KickScooterSharing.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Scooter).Include(p => p.Status).Include(p => p.Tariff);
            return PartialView(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Scooter)
                .Include(p => p.Status)
                .Include(p => p.Tariff)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ScooterId"] = new SelectList(_context.Scooter, "Id", "Number");
            ViewData["TariffId"] = new SelectList(_context.Tariff, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TariffId,ScooterId,Latitude,Longitude")] ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product
                {
                    Name = model.Name,
                    TariffId = model.TariffId,
                    ScooterId = model.ScooterId,
                    StatusId = 1
                };
                _context.Add(product);
                ProductLocation productLocation = new ProductLocation
                {
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Product = product
                };
                _context.Add(productLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Management", "Home");
            }
            ViewData["ScooterId"] = new SelectList(_context.Scooter, "Id", "Number", model.ScooterId);
            ViewData["TariffId"] = new SelectList(_context.Tariff, "Id", "Name", model.TariffId);
            return View(model);
        }

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

            var location = await _context.ProductLocations.Where(s => s.Id == product.Id).FirstOrDefaultAsync();
            ProductViewModel model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                TariffId = product.TariffId,
                ScooterId = product.ScooterId,
                Latitude = location.Latitude,
                Longitude = location.Longitude
            };
            
            ViewData["ScooterId"] = new SelectList(_context.Scooter, "Id", "Number", product.ScooterId);
            ViewData["TariffId"] = new SelectList(_context.Tariff, "Id", "Name", product.TariffId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TariffId,ScooterId,Latitude,Longitude")] ProductViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Product product = _context.Products.Find(id);
                ProductLocation productLocation = _context.ProductLocations.Where(l => l.ProductId == product.Id).FirstOrDefault();

                product.Name = model.Name;
                product.TariffId = model.TariffId;
                product.ScooterId = model.ScooterId;
                productLocation.Latitude = model.Latitude;
                productLocation.Longitude = model.Longitude;

                try
                {
                    _context.Update(product);
                    _context.Update(productLocation);
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
                return RedirectToAction("Management", "Home");
            }
            ViewData["ScooterId"] = new SelectList(_context.Scooter, "Id", "Number", model.ScooterId);
            ViewData["TariffId"] = new SelectList(_context.Tariff, "Id", "Name", model.TariffId);
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Scooter)
                .Include(p => p.Status)
                .Include(p => p.Tariff)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            var productLocation = await _context.ProductLocations.Where(p => p.ProductId == product.Id).FirstOrDefaultAsync();
            if (productLocation != null)
            {
                _context.ProductLocations.Remove(productLocation);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(); 
            return RedirectToAction("Management", "Home");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
