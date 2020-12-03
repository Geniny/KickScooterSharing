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
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Scooter).ThenInclude(p => p.ScooterModel).Include(p => p.Status).Include(p => p.Tariff);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Product/Details/5
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

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["ScooterId"] = new SelectList(_context.Set<Scooter>(), "Id", "Name");
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Name");
            ViewData["TariffId"] = new SelectList(_context.Set<Tariff>(), "Id", "Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["ScooterId"] = new SelectList(_context.Set<Scooter>(), "Id", "Id", product.ScooterId);
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Id", product.StatusId);
            ViewData["TariffId"] = new SelectList(_context.Set<Tariff>(), "Id", "Id", product.TariffId);
            return View(product);
        }

        // GET: Product/Edit/5
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
            ViewData["ScooterId"] = new SelectList(_context.Set<Scooter>(), "Id", "Id", product.ScooterId);
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Id", product.StatusId);
            ViewData["TariffId"] = new SelectList(_context.Set<Tariff>(), "Id", "Id", product.TariffId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["ScooterId"] = new SelectList(_context.Set<Scooter>(), "Id", "Id", product.ScooterId);
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Id", product.StatusId);
            ViewData["TariffId"] = new SelectList(_context.Set<Tariff>(), "Id", "Id", product.TariffId);
            return View(product);
        }

        // GET: Product/Delete/5
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

        // POST: Product/Delete/5
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
