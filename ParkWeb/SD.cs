using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkWeb
{
    public static class SD
    {
        public static string APIBaseURL = "https://localhost:44368/"; 
        public static string ParkAPI_URL = APIBaseURL + "api/v1/nationalparks";
        public static string TraiAPI_lURL = APIBaseURL + "api/v1/trails";
    }
}
