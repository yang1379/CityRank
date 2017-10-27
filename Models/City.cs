using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityRank.Models
{
    public class City : BaseEntity
    {
        public int Id { get; set;}
        public string Name { get; set; }
        public string State { get; set; }
        public List<Bar> Bars { get; set; }
        public List<BicycleStore> BicycleStores { get; set; }
        public List<Cafe> Cafes { get; set; }
        public List<ConvenienceStore> ConvenienceStores { get; set; }
        public List<Park> Parks { get; set; }
        public List<Restaurant> Restaurants { get; set; }
        public List<Starbuck> Starbucks { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        public City() {
            Parks = new List<Park>();
        }
    }
}
