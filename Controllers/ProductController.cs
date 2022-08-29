using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreBackend.Models;
using StoreBackend.Services;

namespace StoreBackend.Controllers {
    
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase {

        [HttpGet]
        public ActionResult<List<Product>> all() {
            return Ok(ProductService.all());
        }

        [HttpGet("{id}")]
        public ActionResult<Product> single(int id){
            
            Product prod = ProductService.view(id);
            if(prod == null){
                return BadRequest("product not found");
            }else{
                return Ok(prod);
            }

        }

        [HttpPost]
        public ActionResult<List<Product>> add(Product new_product){
            return ProductService.addProduct(new_product);
        }

        [HttpPut]
        public ActionResult<List<Product>> edit(Product new_product){
            return ProductService.editProduct(new_product);
        }

    }

}