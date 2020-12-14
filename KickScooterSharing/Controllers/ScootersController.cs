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
    public class ScootersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScootersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Scooters
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Scooter.Include(s => s.ScooterModel);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Scooters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scooter = await _context.Scooter
                .Include(s => s.ScooterModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scooter == null)
            {
                return NotFound();
            }

            return View(scooter);
        }

        // GET: Scooters/Create
        public IActionResult Create()
        {
            ViewData["ScooterModelId"] = new SelectList(_context.Set<ScooterModel>(), "Id", "Id");
            return View();
        }

        // POST: Scooters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Color,Number,ScooterModelId")] Scooter scooter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scooter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScooterModelId"] = new SelectList(_context.Set<ScooterModel>(), "Id", "Id", scooter.ScooterModelId);
            return View(scooter);
        }

        // GET: Scooters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scooter = await _context.Scooter.FindAsync(id);
            if (scooter == null)
            {
                return NotFound();
            }
            ViewData["ScooterModelId"] = new SelectList(_context.Set<ScooterModel>(), "Id", "Id", scooter.ScooterModelId);
            return View(scooter);
        }

        // POST: Scooters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Color,Number,ScooterModelId")] Scooter scooter)
        {
            if (id != scooter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scooter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScooterExists(scooter.Id))
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
            ViewData["ScooterModelId"] = new SelectList(_context.Set<ScooterModel>(), "Id", "Id", scooter.ScooterModelId);
            return View(scooter);
        }

        // GET: Scooters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scooter = await _context.Scooter
                .Include(s => s.ScooterModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scooter == null)
            {
                return NotFound();
            }

            return View(scooter);
        }

        // POST: Scooters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scooter = await _context.Scooter.FindAsync(id);
            _context.Scooter.Remove(scooter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScooterExists(int id)
        {
            return _context.Scooter.Any(e => e.Id == id);
        }
    }
}
