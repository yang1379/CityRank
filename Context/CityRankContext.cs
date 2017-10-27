using Microsoft.EntityFrameworkCore;
 
namespace CityRank.Models
{
    public class CityRankContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public CityRankContext(DbContextOptions<CityRankContext> options) : base(options) { }

        public DbSet<City> Cities { get; set; }
        public DbSet<Bar> Bars { get; set; }
        public DbSet<BicycleStore> BicycleStores { get; set; }
        public DbSet<Cafe> Cafes { get; set; }
        public DbSet<ConvenienceStore> ConvenienceStores { get; set; }
        public DbSet<Park> Parks { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Starbuck> Starbucks { get; set; }
        
    }
}