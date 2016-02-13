using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BjcScraper
{
    public class Category
    {
        public Category()
        {
            Selected = true;
            Childs = new List<Category>();
        }

        public string Url { get; set; }
        public string Title { get; set; }
        public bool Selected { get; set; }
        public List<Category> Childs { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
