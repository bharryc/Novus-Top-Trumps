using Novus_Top_Trumps.Models;

namespace Novus_Top_Trumps.Controllers
{
    public class HeroComparisonViewModel
    {
        public HeroCards Card1 { get; set; }
        public HeroCards Card2 { get; set; }
        public List<HeroCards> Deck1 { get; set; }
        public List<HeroCards> Deck2 { get; set; }
        public string AttributeName { get; set; }
        public bool? IsCard1Winner { get; set; }
        public int Card1AttributeValue { get; set; }
        public int Card2AttributeValue { get; set; }
    }
}
