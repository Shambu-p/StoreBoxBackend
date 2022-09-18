using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StoreBackend.Data;
using StoreBackend.Models;
using StoreBackend.Services.Inventories;
using Microsoft.AspNetCore.Authorization;

namespace StoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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

        [HttpGet("items/{store_id}")]
        public async Task<ActionResult<IEnumerable<StoreItem>>> getAllStoreItems(uint store_id) {

            StoreService service = new StoreService(context);
            return Ok(await service.getAllItems(store_id));
            // return Ok(await context.StoreItems.Include(s => s.Store).Where(s => s.Store.Id == store_id).ToListAsync());

        }

        [HttpGet("{store_id}")]
        public async Task<ActionResult<Store>> getStore(uint store_id){

            StoreService service = new StoreService(context);
            return Ok(await service.getStoreById(store_id));

        }

        [HttpGet("items/single_item/{id}")]
        public async Task<ActionResult<IEnumerable<StoreItem>>> getStoreItem(uint id) {

            StoreItemService service = new StoreItemService(context);
            return Ok(service.storeItem(id));

        }

        [HttpPost]
        public async Task<ActionResult<Store>> addStore([FromForm] string store_name, [FromForm] uint store_keeper) {

            StoreService service = new StoreService(context);
            return Ok(await service.addStore(store_name, store_keeper));                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                

        }

        [HttpPost("items")]
        public async Task<ActionResult<StoreItem>> createStoreItem(uint store_id, uint item_id, uint amount) {

            StoreItemService service = new StoreItemService(context);
            return await service.createItem(store_id, item_id, amount);

        }

        [HttpPut]
        public async Task<ActionResult<Store>> editStore([FromForm] uint store_id, [FromForm] string store_name, [FromForm] uint store_keeper) {

            StoreService service = new StoreService(context);
            Store store = new Store();
            store.Id = store_id;
            store.Name = store_name;
            store.StoreKeeper = store_keeper;

            return await service.changeStore(store);

        }

        [HttpPut("items/add_amount")]
        public async Task<ActionResult<StoreItem>> addStoreItemAmount(uint store_id, uint item_id, uint amount) {

            StoreItemService service = new StoreItemService(context);
            return Ok(await service.addItem(store_id, item_id, amount));

        }

    }

}