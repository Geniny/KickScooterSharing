using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KickScooterSharing.Data;
using KickScooterSharing.Models;

namespace KickScooterSharing.Controllers
{
    public class ScooterModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScooterModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ScooterModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.ScooterModel.ToListAsync());
        }

        // GET: ScooterModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scooterModel = await _context.ScooterModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scooterModel == null)
            {
                return NotFound();
            }

            return View(scooterModel);
        }

        // GET: ScooterModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ScooterModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ScooterModel scooterModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scooterModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scooterModel);
        }

        // GET: ScooterModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scooterModel = await _context.ScooterModel.FindAsync(id);
            if (scooterModel == null)
            {
                return NotFound();
            }
            return View(scooterModel);
        }

        // POST: ScooterModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ScooterModel scooterModel)
        {
            if (id != scooterModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scooterModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScooterModelExists(scooterModel.Id))
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
            return View(scooterModel);
        }

        // GET: ScooterModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var scooterModel = await _context.ScooterModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (scooterModel == null)
            {
                return NotFound();
            }

            return View(scooterModel);
        }

        // POST: ScooterModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var scooterModel = await _context.ScooterModel.FindAsync(id);
            _context.ScooterModel.Remove(scooterModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScooterModelExists(int id)
        {
            return _context.ScooterModel.Any(e => e.Id == id);
        }
    }
}
