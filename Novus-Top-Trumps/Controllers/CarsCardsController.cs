using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Novus_Top_Trumps.Data;
using Novus_Top_Trumps.Models;

namespace Novus_Top_Trumps.Controllers
{
    public class CarsCardsController : Controller
    {
        private readonly CardsDBContext _context;

        public CarsCardsController(CardsDBContext context)
        {
            _context = context;
        }

        // GET: CarsCards
        public async Task<IActionResult> Index()
        {
              return _context.CarsCard != null ? 
                          View(await _context.CarsCard.ToListAsync()) :
                          Problem("Entity set 'CardsDBContext.CarsCard'  is null.");
        }

        // GET: CarsCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CarsCard == null)
            {
                return NotFound();
            }

            var carsCard = await _context.CarsCard
                .FirstOrDefaultAsync(m => m.ID == id);
            if (carsCard == null)
            {
                return NotFound();
            }

            return View(carsCard);
        }

        // GET: CarsCards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CarsCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Speed,Horsepower,Weight,Price")] CarsCard carsCard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carsCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carsCard);
        }

        // GET: CarsCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CarsCard == null)
            {
                return NotFound();
            }

            var carsCard = await _context.CarsCard.FindAsync(id);
            if (carsCard == null)
            {
                return NotFound();
            }
            return View(carsCard);
        }

        // POST: CarsCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Speed,Horsepower,Weight,Price")] CarsCard carsCard)
        {
            if (id != carsCard.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carsCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarsCardExists(carsCard.ID))
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
            return View(carsCard);
        }

        // GET: CarsCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CarsCard == null)
            {
                return NotFound();
            }

            var carsCard = await _context.CarsCard
                .FirstOrDefaultAsync(m => m.ID == id);
            if (carsCard == null)
            {
                return NotFound();
            }

            return View(carsCard);
        }

        // POST: CarsCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CarsCard == null)
            {
                return Problem("Entity set 'CardsDBContext.CarsCard'  is null.");
            }
            var carsCard = await _context.CarsCard.FindAsync(id);
            if (carsCard != null)
            {
                _context.CarsCard.Remove(carsCard);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarsCardExists(int id)
        {
          return (_context.CarsCard?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
