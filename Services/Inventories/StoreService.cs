using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreBackend.Models;
using StoreBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace StoreBackend.Services.Inventories {

    public class StoreService {

        private readonly StoreBackendContext context;

        public StoreService(StoreBackendContext db_context){
            this.context = db_context;
        }

        public async Task<IEnumerable<Store>> getAll() {
            return await context.Stores.ToListAsync();
        }

        public async Task<Store> getStoreById(uint id){
            return await context.Stores.FindAsync(id);
        }

        public async Task<Store> addStore(string name, uint store_keeper) {

            User store_user = await this.getStoreKeeper(store_keeper);

            if(store_user == null) {
                return null;
            }

            Store new_store = new Store();
            new_store.Name = name;
            new_store.StoreKeeper = store_keeper;

            context.Stores.Add(new_store);
            var result = await context.SaveChangesAsync();

            new_store.StoreKeeperNavigation = store_user;

            return new_store;

        }

        public async Task<ActionResult<Store>> changeStore(Store new_change){

            Store store = await this.getStoreById(new_change.Id);
            User store_keeper = null;

            if(store != null){

                if(store.Name != null){
                    store.Name = new_change.Name;
                }

                if(store.StoreKeeper != null){
                    
                    store_keeper = await this.getStoreKeeper(new_change.StoreKeeper);
                    if(store_keeper != null){
                        store.StoreKeeper = new_change.StoreKeeper;
                    }

                }

                context.Entry(store_keeper).State = EntityState.Modified;
                await context.SaveChangesAsync();

                if(store_keeper != null){
                    store.StoreKeeperNavigation = store_keeper;
                }

            }

            return store;

        }

        private async Task<User> getStoreKeeper(uint id){
            StoreBackend.Services.UserService service = new StoreBackend.Services.UserService(context);
            return await service.getUserById(id);
        }
        
    }

}