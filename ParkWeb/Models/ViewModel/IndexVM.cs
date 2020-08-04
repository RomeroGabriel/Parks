using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkWeb.Models.ViewModel
{
    public class IndexVM
    {
        public IEnumerable<NationalPark> NationalParksList { get; set; }
        public IEnumerable<Trail> TrailsList { get; set; }
    }
}
