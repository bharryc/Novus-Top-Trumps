using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Novus_Top_Trumps.Data;
using Novus_Top_Trumps.Models;

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
        public IActionResult Index()
        {
            List<CustomItemModel> customItems = new List<CustomItemModel>
            {
                new CustomItemModel { Link = "../CarsCards/SelectAttribute", Name = "Cars" },
                new CustomItemModel { Link = "../PokemonCards/SelectAttribute", Name = "Pokemon" }
            };
            
            ViewBag.CustomItems = customItems;
            
            return View();
        }
    }
}
