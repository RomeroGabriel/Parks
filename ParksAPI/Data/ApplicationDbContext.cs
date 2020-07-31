using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ParksAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        public DbSet<NationalPark> NationalParks { get; set; }

        public DbSet<Trail> Trails { get; set; }
    }
}
