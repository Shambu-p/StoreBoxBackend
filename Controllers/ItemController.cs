using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreBackend.Services.Items;
using StoreBackend.Models;
using StoreBackend.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace StoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase {
        
        public readonly StoreBackendContext context;
        
        public ItemController (StoreBackendContext store_context) {
            this.context = store_context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> all(){
            ItemService service = new ItemService(context);
            return await service.getAll();
        }

        [HttpGet("{item_id}")]
        public async Task<ActionResult<Item>> getItem(uint item_id) {
            
            ItemService service = new ItemService(context);
            var item = await service.getItemById(item_id);
            
            if(item == null){
                return NotFound("Item not found!");
            }

            return item;

        }

        [HttpPost]
        public async Task<ActionResult<Item>> addItem(string name, double price){
            ItemService service = new ItemService(context);
            return await service.addItem(name, price);
        }

        [HttpPut]
        public async Task<ActionResult<Item>> changeItem([FromBody] Item new_item){
            
            ItemService service = new ItemService(context);
            Item item = await service.changeItem(new_item);

            if(item == null){
                return NotFound("item not found!");
            }

            return item;

        }

    }

}