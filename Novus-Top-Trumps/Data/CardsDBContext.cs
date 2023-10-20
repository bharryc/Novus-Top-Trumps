using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Novus_Top_Trumps.Models;

namespace Novus_Top_Trumps.Data
{
    public class CardsDBContext : DbContext
    {
        public CardsDBContext (DbContextOptions<CardsDBContext> options)
            : base(options)
        {
        }

        public DbSet<Novus_Top_Trumps.Models.CarsCards> CarsCard { get; set; } = default!;
        public DbSet<Novus_Top_Trumps.Models.PokemonCards> PokemonCard { get; set; } = default!;
        public DbSet<Novus_Top_Trumps.Models.UFCCards> UFCCard { get; set; } = default!;
        public DbSet<Novus_Top_Trumps.Models.HeroCards> HeroCard { get; set; } = default!;
    }
}
