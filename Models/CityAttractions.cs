using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityRank.Models
{
    public class CityAttractions : BaseEntity
    {
        public string Attraction { get; set; }
        public List<Attraction> Attractions { get; set; }
        public string NextPageToken { get; set; }

        public CityAttractions() {
            Attractions = new List<Attraction>();
        }
    }
}
