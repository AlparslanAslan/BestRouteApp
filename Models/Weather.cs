using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestWayWebApp.Models
{
    public class Weather
    {
       
            public string apiResponse { get; set; }

            public Dictionary<string, string> cities
            {
                get; set;
            }
        
    }
}
