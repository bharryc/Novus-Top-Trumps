using Microsoft.AspNetCore.Mvc;
using Novus_Top_Trumps.Data;
using Novus_Top_Trumps.Models;

namespace Novus_Top_Trumps.Controllers
{
    public class DeckSelectorController : Controller
    {

        public IActionResult DeckSelection()
        {
            return View();
        }
        public IActionResult Index()
        {
            
            List<CustomItemModel> customItems = new List<CustomItemModel>
            {
                new CustomItemModel { Link = "../CarsCards", Name = "Cars" },
                new CustomItemModel { Link = "../PokemonCards", Name = "Pokemon" }
            };
            
            ViewBag.CustomItems = customItems;
            
            return View();
        }
    }
}
