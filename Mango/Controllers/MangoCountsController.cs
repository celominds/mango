using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mango.Models;

namespace Mango.Controllers
{
    public class MangoCountsController : Controller
    {

        MangoDBContext _context = new MangoDBContext();

        public async Task<IActionResult> Index()
        {
            return View(await _context.MangoCount.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mangoCount = await _context.MangoCount
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mangoCount == null)
            {
                return NotFound();
            }

            return View(mangoCount);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Year,Count")] MangoCount mangoCount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mangoCount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mangoCount);
        }

        // GET: MangoCounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mangoCount = await _context.MangoCount.FindAsync(id);
            if (mangoCount == null)
            {
                return NotFound();
            }
            return View(mangoCount);
        }

        // POST: MangoCounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Year,Count")] MangoCount mangoCount)
        {
            if (id != mangoCount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mangoCount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MangoCountExists(mangoCount.Id))
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
            return View(mangoCount);
        }

        // GET: MangoCounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mangoCount = await _context.MangoCount
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mangoCount == null)
            {
                return NotFound();
            }

            return View(mangoCount);
        }

        // POST: MangoCounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mangoCount = await _context.MangoCount.FindAsync(id);
            _context.MangoCount.Remove(mangoCount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MangoCountExists(int id)
        {
            return _context.MangoCount.Any(e => e.Id == id);
        }
    }
}
