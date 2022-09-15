using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Data;
using StoreBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace StoreBackend.Services.Inventories {

    public class StoreItemService {

        private readonly StoreBackendContext context;
        
        public StoreItemService(StoreBackendContext db_context){
            this.context = db_context;
        }

        public async Task<IEnumerable<StoreItem>> getAllItems(uint store_id) {
            return await context.StoreItems
                                .Include(si => si.Store) //.ThenInclude(store => store.StoreKeeperNavigation)
                                .Include(si => si.Item)
                                .Where(si => si.StoreId == store_id).ToListAsync();
        }

        public StoreItem storeItem(uint id) {
            return context.StoreItems.Include(si => si.Store).Include(si => si.Item).Where(si => si.Id == id).FirstOrDefault();
        }

        public async Task<ActionResult<StoreItem>> createItem(uint store_id, uint item_id, uint total_amount) {

            // Store store = await this.getStore(store_id);
            var store = context.Stores.Include(s => s.StoreItems).Where(s => s.Id == store_id).First();
            Item item = await this.getItem(item_id);

            if(store == null && item == null){
                return null;
            }


            StoreItem new_store_item = new StoreItem();
            new_store_item.StoreId = store_id;
            new_store_item.ItemId = item_id;
            new_store_item.TotalAmount = total_amount;
            new_store_item.UnboxedAmount = total_amount;
            // new_store_item.Store = store;

            store.StoreItems.Add(new_store_item);
            await context.SaveChangesAsync();

            new_store_item.Item = item;

            return new_store_item;

        }

        public async Task<StoreItem> addItem(uint item_id, uint store_id, uint new_amount) {

            // StoreItem store_item = this.getStoreItem(item_id, store_id);
            StoreItem store_item = this.context.StoreItems.Where(st => st.ItemId == item_id && st.StoreId == store_id).FirstOrDefault();

            if(store_item != null) {
                store_item.TotalAmount = store_item.TotalAmount + new_amount;
                store_item.UnboxedAmount = store_item.UnboxedAmount + new_amount;
                this.context.SaveChanges();
                // return await this.changeItem(store_item);
                // return store_item;
            }

            return store_item;

        }

        public async Task<StoreItem> itemBoxing(uint item_id, uint store_id, uint new_amount) {
            
            StoreItem store_item = this.getStoreItem(item_id, store_id);

            if(store_item != null) {

                if(store_item.UnboxedAmount >= new_amount) {
                    store_item.UnboxedAmount -= new_amount;    
                }

                return await this.changeItem(store_item);

            }

            return store_item;

        }

        public async Task<StoreItem> itemUnboxing(uint store_id, uint item_id, uint new_amount) {

            StoreItem store_item = this.getStoreItem(item_id, store_id);

            if(store_item != null) {

                store_item.UnboxedAmount += new_amount;

                return await this.changeItem(store_item);

            }

            return store_item;

        }

        public async Task<StoreItem> changeItem(StoreItem change_item){

            context.Entry(change_item).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return change_item;

        }

        public StoreItem getStoreItem(uint item_id, uint store_id){
            return this.context.StoreItems.Where(st => st.ItemId == item_id && st.StoreId == store_id).FirstOrDefault<StoreItem>();
        }

        public async Task<StoreItem> getStoreItemById(uint id){
            return await this.context.StoreItems.FindAsync(id);
        }

        private async Task<Store> getStore(uint store_id) {
            StoreService service = new StoreService(this.context);
            return await service.getStoreById(store_id);
        }

        private async Task<Item> getItem(uint store_id) {
            StoreBackend.Services.Items.ItemService service = new StoreBackend.Services.Items.ItemService(this.context);
            return await service.getItemById(store_id);
        }

        private async Task<User> getUser(uint id){
            StoreBackend.Services.UserService service = new StoreBackend.Services.UserService(this.context);
            return await service.getUserById(id);
        }

    }

}