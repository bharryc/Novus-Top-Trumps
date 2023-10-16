using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
            List<int> deck1;
            List<int> deck2;

            // Check if decks exist in TempData
            if (TempData["Deck1"] == null || TempData["Deck2"] == null)
            {
                // Retrieve all card IDs
                var allCardIds = await _context.CarsCard.Select(card => card.ID).ToListAsync();

                // Check for insufficient cards
                if (allCardIds.Count < 2)
                {
                    return View("Error", new ErrorViewModel { RequestId = "Not enough cards to compare" });
                }

                var rand = new Random();

                // Shuffle all cards (Fisher-Yates shuffle)
                for (int i = allCardIds.Count - 1; i > 0; i--)
                {
                    int j = rand.Next(i + 1);
                    var temp = allCardIds[i];
                    allCardIds[i] = allCardIds[j];
                    allCardIds[j] = temp;
                }

                // Split into two decks
                var halfIndex = allCardIds.Count / 2;
                deck1 = allCardIds.Take(halfIndex).ToList();
                deck2 = allCardIds.Skip(halfIndex).ToList();

                // Store the decks in TempData
                TempData["Deck1"] = JsonConvert.SerializeObject(deck1);
                TempData["Deck2"] = JsonConvert.SerializeObject(deck2);
            }
            else
            {
                deck1 = JsonConvert.DeserializeObject<List<int>>(TempData["Deck1"].ToString());
                deck2 = JsonConvert.DeserializeObject<List<int>>(TempData["Deck2"].ToString());

                TempData.Keep("Deck1");
                TempData.Keep("Deck2");
            }

            // Default attribute if one is not provided
            attributeName = attributeName ?? "speed";

            var card1 = await _context.CarsCard.FindAsync(deck1.First());
            var card2 = await _context.CarsCard.FindAsync(deck2.First());

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

            var viewModel = new CardComparisonViewModel
            {
                Card1 = card1,
                Card2 = card2,
                Deck1 = await _context.CarsCard.Where(c => deck1.Contains(c.ID)).ToListAsync(),
                Deck2 = await _context.CarsCard.Where(c => deck2.Contains(c.ID)).ToListAsync(),
                AttributeName = attributeName,
                IsCard1Winner = isCard1Winner,
                Card1AttributeValue = card1AttributeValue,
                Card2AttributeValue = card2AttributeValue
            };

            return View(viewModel);
        }


        private bool CarsCardExists(int id)
        {
          return (_context.CarsCard?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        [HttpPost]
        public IActionResult SelectAttributeForComparison(string attributeName)
        {
            // Simple validation: check if attributeName is one of the allowed values
            var validAttributes = new[] { "speed", "horsepower", "weight", "price" };
            if (!validAttributes.Contains(attributeName?.ToLower()))
            {
                return BadRequest("Invalid attribute selected");
            }

            return RedirectToAction("CompareCards", new { attributeName });
        }

        public async Task<IActionResult> SelectAttribute()
        {
            if (TempData["Deck1"] == null || TempData["Deck2"] == null)
            {
                await InitializeDecks();
            }

            var deck1 = JsonConvert.DeserializeObject<List<int>>(TempData["Deck1"].ToString());
            var deck2 = JsonConvert.DeserializeObject<List<int>>(TempData["Deck2"].ToString());

            var card1 = await _context.CarsCard.FindAsync(deck1.First());
            var card2 = await _context.CarsCard.FindAsync(deck2.First());

            var viewModel = new CardComparisonViewModel
            {
                Card1 = card1,
                Card2 = card2
            };

            return View(viewModel);
        }

        private async Task InitializeDecks()
{
    // Retrieve all card IDs
    var allCardIds = await _context.CarsCard.Select(card => card.ID).ToListAsync();

    // Check for insufficient cards
    if (allCardIds.Count < 2)
    {
        throw new InvalidOperationException("Not enough cards to compare");
    }

    var rand = new Random();

    // Shuffle all cards (Fisher-Yates shuffle)
    for (int i = allCardIds.Count - 1; i > 0; i--)
    {
        int j = rand.Next(i + 1);
        var temp = allCardIds[i];
        allCardIds[i] = allCardIds[j];
        allCardIds[j] = temp;
    }

    // Split into two decks
    var halfIndex = allCardIds.Count / 2;
    var deck1 = allCardIds.Take(halfIndex).ToList();
    var deck2 = allCardIds.Skip(halfIndex).ToList();

    TempData["Deck1"] = JsonConvert.SerializeObject(deck1);
    TempData["Deck2"] = JsonConvert.SerializeObject(deck2);
}


        public async Task<IActionResult> DisplayTopCards()
        {
            InitializeDecks();

            var deck1 = JsonConvert.DeserializeObject<List<int>>(TempData["Deck1"].ToString());
            var deck2 = JsonConvert.DeserializeObject<List<int>>(TempData["Deck2"].ToString());

            var card1 = await _context.CarsCard.FindAsync(deck1.First());
            var card2 = await _context.CarsCard.FindAsync(deck2.First());

            var viewModel = new CardComparisonViewModel
            {
                Card1 = card1,
                Card2 = card2,
                // Populate other required properties if needed
            };

            return View("DisplayTopCardsView", viewModel);
        }



    }
}
