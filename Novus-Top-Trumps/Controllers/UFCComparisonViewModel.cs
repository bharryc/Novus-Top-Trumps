using Novus_Top_Trumps.Models;

namespace Novus_Top_Trumps.Controllers
{
    public class UFCComparisonViewModel
    {
        public UFCCards Card1 { get; set; }
        public UFCCards Card2 { get; set; }
        public List<UFCCards> Deck1 { get; set; }
        public List<UFCCards> Deck2 { get; set; }
        public string AttributeName { get; set; }
        public bool? IsCard1Winner { get; set; }
        public int Card1AttributeValue { get; set; }
        public int Card2AttributeValue { get; set; }
    }
}
