using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Novus_Top_Trumps.Data;
using Novus_Top_Trumps.Models;
using System.Text.Json;

namespace Novus_Top_Trumps.Controllers
{
    public class DeckSelectorController : Controller
    {

        [Authorize]
        public IActionResult DeckSelection()
        {
            return View();
        }

        [Authorize]
        public IActionResult Index(string difficulty)
        {
            TempData["Difficulty"] = difficulty;

            List<CustomItemModel> customItems = new List<CustomItemModel>
            {
                new CustomItemModel { Link = "../CarsCards/SelectAttribute", Name = "Cars" },
                new CustomItemModel { Link = "../PokemonCards/SelectAttribute", Name = "Pokemon" }
            };
            
            ViewBag.CustomItems = customItems;
            
            return View();
        }
        [Authorize]
        public IActionResult DifficultySelect()
        {
            return View();
        }
    }
}
