using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityRank.Models
{
    public class Attraction : BaseEntity
    {
        public int Id { get; set;}
        public int CityId{ get; set; }
        public string Name { get; set; }
        public string Vicinity { get; set; }
        public double Latitude { get; set; } 
        public double Longitude { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }
    }
}
