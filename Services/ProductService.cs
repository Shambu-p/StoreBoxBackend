using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Models;

namespace StoreBackend.Services
{
    public class ProductService
    {
        
        static List<Product> product_list = new List<Product>();

        public static List<Product> all(){
            return product_list;
        }

        public static Product view(int id) {
            var product = product_list.Find(p => p.id == id);
            return product;
        }

        public static List<Product> addProduct(Product prod) {

            product_list.Add(prod);
            return product_list;

        }

        public static List<Product> editProduct(Product prod){
            var product = product_list.Find(p => p.id == prod.id);
            return product_list;
        }

    }
}