using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Data;
using StoreBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StoreBackend.Services.Cartoons;
using StoreBackend.Services.Inventories;

namespace StoreBackend.Services.Cartoons {
    
    public class BoxItemService {

        private readonly StoreBackendContext context;

        public BoxItemService(StoreBackendContext db_context){
            this.context = db_context;
        }

        public async Task<IEnumerable<BoxItem>> getByBox(uint box_id) {
            return await context.BoxItems.Where((b => b.BoxId == box_id)).ToListAsync();
        }

        public async Task<BoxItem> single(uint box_id, uint item_id) {
            return context.BoxItems.Include(b => b.Box).Include(b => b.Item).Where(b => b.BoxId == box_id && b.ItemId == item_id).FirstOrDefault();
        }

        public async Task<BoxItem> create(uint box_id, uint item_id, int amount){

            // BoxService box_service = new BoxService(context);
            StoreItemService store_item_service = new StoreItemService(context);

            Box bx = context.Boxes.Include(b => b.Store).Where(b => b.Id == box_id).FirstOrDefault();
            if(bx == null){
                return null;
            }

            StoreItem st = store_item_service.getStoreItem(item_id, bx.Store.Id);
            
            if(st.UnboxedAmount == 0 || st.UnboxedAmount < amount){
                return null;
            }

            BoxItem bx_item = new BoxItem();
            bx_item.BoxId = box_id;
            bx_item.ItemId = item_id;
            bx_item.Amount = amount;

            await store_item_service.itemBoxing(item_id, bx.Store.Id, Convert.ToUInt32(amount));

            context.BoxItems.Add(bx_item);
            await context.SaveChangesAsync();

            return bx_item;

        }

        public async Task<BoxItem> addBoxItem(uint box_id, uint item_id, int amount){

            StoreItemService store_item_service = new StoreItemService(context);

            Box bx = context.Boxes.Include(b => b.Store).Where(b => b.Id == box_id).FirstOrDefault();
            if(bx == null){
                return null;
            }

            StoreItem st = store_item_service.getStoreItem(item_id, bx.Store.Id);
            
            if(st.UnboxedAmount == 0 || st.UnboxedAmount < amount){
                return null;
            }

            BoxItem bx_item = context.BoxItems.Where(b => b.BoxId == box_id && b.ItemId == item_id).FirstOrDefault();

            if(bx_item == null){
                return null;
            }

            bx_item.Amount += amount;
            await store_item_service.itemBoxing(item_id, bx.Store.Id, Convert.ToUInt32(amount));
            await context.SaveChangesAsync();
            return bx_item;

        }

        public async Task<BoxItem> UnboxItem(uint box_id, uint item_id, int amount){

            StoreItemService store_item_service = new StoreItemService(context);

            Box bx = context.Boxes.Include(b => b.Store).Where(b => b.Id == box_id).FirstOrDefault();
            if(bx == null){
                return null;
            }

            // StoreItem st = store_item_service.getStoreItem(item_id, bx.Store.Id);
            
            // if(st.UnboxedAmount == 0 || st.UnboxedAmount < amount){
            //     return null;
            // }

            BoxItem bx_item = context.BoxItems.Where(b => b.BoxId == box_id && b.ItemId == item_id).FirstOrDefault();

            if(bx_item == null || bx_item.Amount < amount){
                return null;
            }

            bx_item.Amount -= amount;
            await store_item_service.itemUnboxing(item_id, bx.Store.Id, Convert.ToUInt32(amount));
            await context.SaveChangesAsync();
            return bx_item;

        }
        
    }

}