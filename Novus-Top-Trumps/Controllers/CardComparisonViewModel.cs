using Novus_Top_Trumps.Models;

namespace Novus_Top_Trumps.Controllers
{
    internal class CardComparisonViewModel
    {
        public CarsCards Card1 { get; set; }
        public CarsCards Card2 { get; set; }
        public List<CarsCards> Deck1 { get; set; }
        public List<CarsCards> Deck2 { get; set; }
        public string AttributeName { get; set; }
        public bool? IsCard1Winner { get; set; }
        public int Card1AttributeValue { get; set; }
        public int Card2AttributeValue { get; set; }
    }

}