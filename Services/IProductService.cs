using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
   public interface IProductService
    {
        IEnumerable<Product> GetProducts();
        Product GetProduct(int id);
        void Edit(int id, Product product);
        void Add(Product product);
        void Delete(int id);
    }
}
