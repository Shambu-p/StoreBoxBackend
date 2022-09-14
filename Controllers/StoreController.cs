using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StoreBackend.Data;
using StoreBackend.Models;
using StoreBackend.Services.Inventories;

namespace StoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase {
        
        private readonly StoreBackendContext context;

        public StoreController(StoreBackendContext db_context){
            this.context = db_context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> getAllStores() {

            StoreService service = new StoreService(context);
            return Ok(await service.getAll());

        }

        [HttpGet("store_items/{store_id}")]
        public async Task<ActionResult<IEnumerable<StoreItem>>> getAllStoreItems(uint store_id) {

            // StoreItemService service = new StoreItemService(context);
            // return Ok(await service.getAllItems(store_id));
            return Ok(await context.Stores.Include(s => s.StoreItems).Where(s => s.Id == store_id).ToListAsync());

        }

        [HttpGet("{store_id}")]
        public async Task<ActionResult<Store>> getStore(uint store_id){

            StoreService service = new StoreService(context);
            return Ok(await service.getStoreById(store_id));

        }

        [HttpGet("store_items/{store_id}/{item_id}")]
        public async Task<ActionResult<IEnumerable<StoreItem>>> getAllStoreItems(uint store_id, uint item_id) {

            StoreItemService service = new StoreItemService(context);
            return Ok(service.storeItem(store_id, item_id));

        }

        [HttpPost]
        public async Task<ActionResult<Store>> addStore(string store_name, uint store_keeper) {

            StoreService service = new StoreService(context);
            return Ok(await service.addStore(store_name, store_keeper));

        }

        [HttpPost("store_items")]
        public async Task<ActionResult<StoreItem>> createStoreItem(uint store_id, uint item_id, uint amount) {

            StoreItemService service = new StoreItemService(context);
            return await service.createItem(store_id, item_id, amount);

        }

        [HttpPut]
        public async Task<ActionResult<Store>> editStore(uint store_id, string store_name, uint store_keeper) {

            StoreService service = new StoreService(context);
            Store store = new Store();
            store.Id = store_id;
            store.Name = store_name;
            store.StoreKeeper = store_keeper;

            return await service.changeStore(store);

        }

        [HttpPut("store_items/add_amount")]
        public async Task<ActionResult<StoreItem>> addStoreItemAmount(uint store_id, uint item_id, uint amount) {

            StoreItemService service = new StoreItemService(context);
            return Ok(await service.createItem(store_id, item_id, amount));

        }

        [HttpPut("store_items/item_boxing")]
        public async Task<ActionResult<StoreItem>> boxingItem(uint store_id, uint item_id, uint amount) {

            StoreItemService service = new StoreItemService(context);
            return Ok(await service.itemBoxing(item_id, store_id, amount));

        }

        [HttpPut("store_items/item_unboxing")]
        public async Task<ActionResult<StoreItem>> unboxingItem(uint store_id, uint item_id, uint amount) {

            StoreItemService service = new StoreItemService(context);
            return Ok(await service.itemUnboxing(item_id, store_id, amount));

        }

    }

}