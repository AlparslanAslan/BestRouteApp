using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestWayWebApp.Models
{
    public class SensorRecord
    {
        public DateTime Datetime { get; set; }
        public long Longitute { get; set; }
        public long Latitude { get; set; }
        public string Geohash { get; set; }
        public float MinimumSpeed { get; set; }
        public float MaximumSpeed { get; set; }
        public float AverageSpeed { get; set; }
        public int NumberOfVehicles { get; set; }
    }
}
