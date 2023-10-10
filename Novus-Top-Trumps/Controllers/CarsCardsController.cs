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

        public IActionResult CarsCards()
        {
            return View();
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
        public async Task<IActionResult> Create([Bind("ID,Name,Speed,Horsepower,Weight,Price")] CarsCards carsCard)
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Speed,Horsepower,Weight,Price")] CarsCards carsCard)
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

        public async Task<IActionResult> CompareCards(string attributeName)
        {
            int minId = await _context.CarsCard.MinAsync(card => card.ID);
            int maxId = await _context.CarsCard.MaxAsync(card => card.ID);

            // Check for insufficient cards
            if (minId == maxId)
            {
                // Consider redirecting to a user-friendly error page or displaying a message.
                return View("Error", new ErrorViewModel { RequestId = "Not enough cards to compare" });
            }

            int? card1Id, card2Id;
            do
            {
                card1Id = new Random().Next(minId, maxId + 1);
                card2Id = new Random().Next(minId, maxId + 1);
            } while (card1Id == card2Id || await _context.CarsCard.FindAsync(card1Id) == null || await _context.CarsCard.FindAsync(card2Id) == null);

            // Default attribute if one is not provided
            attributeName = attributeName ?? "speed";

            var card1 = await _context.CarsCard.FindAsync(card1Id);
            var card2 = await _context.CarsCard.FindAsync(card2Id);

            if (card1 == null || card2 == null)
            {
                return NotFound();
            }

            int card1AttributeValue;
            int card2AttributeValue;
            bool? isCard1Winner = null;

            // Compare attributes
            switch (attributeName.ToLower())
            {
                case "speed":
                    card1AttributeValue = card1.Speed;
                    card2AttributeValue = card2.Speed;
                    isCard1Winner = card1.Speed > card2.Speed;
                    break;
                case "horsepower":
                    card1AttributeValue = card1.Horsepower;
                    card2AttributeValue = card2.Horsepower;
                    isCard1Winner = card1.Horsepower > card2.Horsepower;
                    break;
                case "weight":
                    card1AttributeValue = card1.Weight;
                    card2AttributeValue = card2.Weight;
                    isCard1Winner = card1.Weight > card2.Weight;
                    break;
                case "price":
                    card1AttributeValue = card1.Price;
                    card2AttributeValue = card2.Price;
                    isCard1Winner = card1.Price > card2.Price;
                    break;
                default:
                    return BadRequest("Invalid attribute name");
            }

            // Populate ViewData
            ViewData["IsCard1Winner"] = isCard1Winner;
            ViewData["Card1AttributeValue"] = card1AttributeValue;
            ViewData["Card2AttributeValue"] = card2AttributeValue;
            ViewData["Card1Name"] = card1.Name;
            ViewData["Card2Name"] = card2.Name;
            ViewData["AttributeName"] = attributeName;

            return View();
        }

        private bool CarsCardExists(int id)
        {
          return (_context.CarsCard?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
