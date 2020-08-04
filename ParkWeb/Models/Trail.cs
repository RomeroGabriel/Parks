using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkWeb.Models
{
    public class Trail
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        [Required]
        public double Elevation { get; set; }
        public TrailDifficulty Difficulty { get; set; }
        [Required]
        public int NationalParkId { get; set; }
        public NationalPark NationalPark { get; set; }
    }

    public enum TrailDifficulty
    {
        Easy,
        Moderate,
        Difficult,
        Expert,
    }
}
