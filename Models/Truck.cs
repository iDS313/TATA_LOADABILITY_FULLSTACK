using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loadability.Models
{
    public class Truck
    {
        public int TruckId { get; set; }
        public string Name { get; set; }
        public decimal Capacity { get; set; }
        public decimal Limit { get; set; }
    }
}