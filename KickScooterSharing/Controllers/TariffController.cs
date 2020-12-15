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

    [Authorize(Roles = "admin")]
    public class TariffController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TariffController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tariff
        public async Task<IActionResult> Index()
        {
            return PartialView(await _context.Tariff.ToListAsync());
        }

        // GET: Tariff/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tariff = await _context.Tariff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tariff == null)
            {
                return NotFound();
            }

            return View(tariff);
        }

        // GET: Tariff/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tariff/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CostPerMinute,StartPrice,BookingCostPerMinute")] Tariff tariff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tariff);
                await _context.SaveChangesAsync();
                return RedirectToAction("Management", "Home");
            }
            return View(tariff);
        }

        // GET: Tariff/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tariff = await _context.Tariff.FindAsync(id);
            if (tariff == null)
            {
                return NotFound();
            }
            return View(tariff);
        }

        // POST: Tariff/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CostPerMinute,StartPrice,BookingCostPerMinute")] Tariff tariff)
        {
            if (id != tariff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tariff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TariffExists(tariff.Id))
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
            return View(tariff);
        }

        // GET: Tariff/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tariff = await _context.Tariff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tariff == null)
            {
                return NotFound();
            }

            return View(tariff);
        }

        // POST: Tariff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tariff = await _context.Tariff.FindAsync(id);
            _context.Tariff.Remove(tariff);
            await _context.SaveChangesAsync();
            return RedirectToAction("Management", "Home");
        }

        private bool TariffExists(int id)
        {
            return _context.Tariff.Any(e => e.Id == id);
        }
    }
}
