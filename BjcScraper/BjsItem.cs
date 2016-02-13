using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BjcScraper
{
    public class BjsItem
    {
        public string Category { get; set; }
        public string Title { get; set; }
        public string ItemNumber { get; set; }
        public double Price { get; set; }
        public string EstimatedDelivery { get; set; }
        public string Description { get; set; }
    }
}
