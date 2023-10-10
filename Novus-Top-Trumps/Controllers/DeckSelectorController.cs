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
                new CustomItemModel { Id = 1, Name = "Cars" },
                new CustomItemModel { Id = 2, Name = "Pokemon" }
            };
            

            ViewData["Decks"] = customItems;
            
            return View();
        }

        [HttpPost]
        public IActionResult Index(FormCollection form)
        {
            ViewData["DropDownId"] = Convert.ToInt32(form["CustomItemDropdown"]);

            // Perform actions based on the selected item
            

            return View();
        }
        
    }
}
