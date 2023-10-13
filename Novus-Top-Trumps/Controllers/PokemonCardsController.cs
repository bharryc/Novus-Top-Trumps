using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Novus_Top_Trumps.Data;
using Novus_Top_Trumps.Models;

namespace Novus_Top_Trumps.Controllers
{
    public class PokemonCardsController : Controller
    {
        private readonly CardsDBContext _context;

        public PokemonCardsController(CardsDBContext context)
        {
            _context = context;
        }

        public IActionResult PokemonCards()
        {
            return View();
        }

        // GET: PokemonCards
        public async Task<IActionResult> Index()
        {
            return _context.PokemonCard != null ?
                        View(await _context.PokemonCard.ToListAsync()) :
                        Problem("Entity set 'CardsDBContext.PokemonCard'  is null.");
        }

        // GET: PokemonCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PokemonCard == null)
            {
                return NotFound();
            }

            var pokemonCard = await _context.PokemonCard
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pokemonCard == null)
            {
                return NotFound();
            }

            return View(pokemonCard);
        }

        // GET: PokemonCards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PokemonCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Speed,Horsepower,Weight,Price")] PokemonCards pokemonCard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pokemonCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pokemonCard);
        }

        // GET: PokemonCards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PokemonCard == null)
            {
                return NotFound();
            }

            var pokemonCard = await _context.PokemonCard.FindAsync(id);
            if (pokemonCard == null)
            {
                return NotFound();
            }
            return View(pokemonCard);
        }

        // POST: PokemonCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Speed,Horsepower,Weight,Price")] PokemonCards pokemonCard)
        {
            if (id != pokemonCard.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pokemonCard);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PokemonCardExists(pokemonCard.ID))
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
            return View(pokemonCard);
        }

        // GET: PokemonCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PokemonCard == null)
            {
                return NotFound();
            }

            var pokemonCard = await _context.PokemonCard
                .FirstOrDefaultAsync(m => m.ID == id);
            if (pokemonCard == null)
            {
                return NotFound();
            }

            return View(pokemonCard);
        }

        // POST: PokemonCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PokemonCard == null)
            {
                return Problem("Entity set 'CardsDBContext.PokemonCard'  is null.");
            }
            var pokemonCard = await _context.PokemonCard.FindAsync(id);
            if (pokemonCard != null)
            {
                _context.PokemonCard.Remove(pokemonCard);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CompareCards(string attributeName)
        {
            // Retrieve all card IDs
            var allCardIds = await _context.PokemonCard.Select(card => card.ID).ToListAsync();

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
            var deck1 = allCardIds.Take(halfIndex).ToList();
            var deck2 = allCardIds.Skip(halfIndex).ToList();

            // Default attribute if one is not provided
            attributeName = attributeName ?? "speed";

            var card1 = await _context.PokemonCard.FindAsync(deck1.First());
            var card2 = await _context.PokemonCard.FindAsync(deck2.First());

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
                case "attack":
                    card1AttributeValue = card1.Attack;
                    card2AttributeValue = card2.Attack;
                    isCard1Winner = card1.Attack > card2.Attack;
                    break;
                case "defence":
                    card1AttributeValue = card1.Defence;
                    card2AttributeValue = card2.Defence;
                    isCard1Winner = card1.Defence > card2.Defence;
                    break;
                case "health":
                    card1AttributeValue = card1.Health;
                    card2AttributeValue = card2.Health;
                    isCard1Winner = card1.Health > card2.Health;
                    break;
                default:
                    return BadRequest("Invalid attribute name");
            }

            var viewModel = new PokemonComparisonViewModel
            {
                Card1 = card1,
                Card2 = card2,
                Deck1 = await _context.PokemonCard.Where(c => deck1.Contains(c.ID)).ToListAsync(),
                Deck2 = await _context.PokemonCard.Where(c => deck2.Contains(c.ID)).ToListAsync(),
                AttributeName = attributeName,
                IsCard1Winner = isCard1Winner,
                Card1AttributeValue = card1AttributeValue,
                Card2AttributeValue = card2AttributeValue
            };

            return View(viewModel);
        }


        private bool PokemonCardExists(int id)
        {
            return (_context.PokemonCard?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        [HttpPost]
        public IActionResult SelectAttributeForComparison(string attributeName)
        {
            // Simple validation: check if attributeName is one of the allowed values
            var validAttributes = new[] { "speed", "attack", "defence", "health" };
            if (!validAttributes.Contains(attributeName?.ToLower()))
            {
                return BadRequest("Invalid attribute selected");
            }

            return RedirectToAction("CompareCards", new { attributeName });
        }

        public IActionResult SelectAttribute()
        {
            return View();
        }

    }
}
