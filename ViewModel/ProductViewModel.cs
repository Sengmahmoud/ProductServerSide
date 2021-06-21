using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModel
{
    public class ProductViewModel
    {
        public IEnumerable<Product>Products { get; init; }
        public Product Product { get; set; }
        public string Img64 { get; set; }

    }
}
