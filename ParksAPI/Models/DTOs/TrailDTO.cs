﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParksAPI.Models.DTOs
{
    public class TrailDTO
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
        public NationalParkDTO NationalPark { get; set; }
    }

    public class TrailCreatelDTO
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }
        [Required]
        public double Elevation { get; set; }
        public TrailDifficulty Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }
    }

    public class TrailUpdatelDTO
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
    }
}
