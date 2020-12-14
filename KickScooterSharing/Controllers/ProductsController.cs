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
            ViewData["ScooterId"] = new SelectList(_context.Scooter, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Id");
            ViewData["TariffId"] = new SelectList(_context.Tariff, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,TariffId,ScooterId,StatusId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScooterId"] = new SelectList(_context.Scooter, "Id", "Id", product.ScooterId);
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Id", product.StatusId);
            ViewData["TariffId"] = new SelectList(_context.Tariff, "Id", "Id", product.TariffId);
            return View(product);
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
            ViewData["ScooterId"] = new SelectList(_context.Scooter, "Id", "Id", product.ScooterId);
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Id", product.StatusId);
            ViewData["TariffId"] = new SelectList(_context.Tariff, "Id", "Id", product.TariffId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,TariffId,ScooterId,StatusId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["ScooterId"] = new SelectList(_context.Scooter, "Id", "Id", product.ScooterId);
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Id", product.StatusId);
            ViewData["TariffId"] = new SelectList(_context.Tariff, "Id", "Id", product.TariffId);
            return View(product);
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
