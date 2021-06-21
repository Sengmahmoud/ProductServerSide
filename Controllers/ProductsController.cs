using API.Models;
using API.Services;
using API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        public IActionResult Get()
        {
            var model = new ProductViewModel()
            {
                Products = _productService.GetProducts()
            };
            return Ok(model.Products);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var model = new ProductViewModel()
            {
                Product = _productService.GetProduct(id)
            };
            return Ok(model);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public IActionResult Post([FromBody] ProductViewModel model)
        {
           

            string imgName = Guid.NewGuid().ToString() ;
            model.Product.ImgName = imgName;
          var filepath =  new PhysicalFileProvider(
                                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "products"))
                                .Root + $@"\{imgName}";

             System.IO.File.WriteAllBytes(filepath, Convert.FromBase64String(model.Img64));
          
        
        
            _productService.Add(model.Product);
            return Ok();
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody]Product product)
        {
                     
            _productService.Edit(id,product);
            return Ok("updated");

        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);
            return Ok("delede");
        }
    }
}
