using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityRank.Models
{
    public class LatLng : BaseEntity
    {
        public int Id { get; set;}
        public string City { get; set; }
        public string State { get; set; }
        public double Latitude { get; set; } 
        public double Longitude { get; set; }
    }
}
