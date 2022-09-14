using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Data;
using StoreBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace StoreBackend.Services.Cartoons {

    public class BoxService {

        private readonly StoreBackendContext context;

        public BoxService(StoreBackendContext db_context){
            this.context = db_context;
        }

        public async Task<Box> viewBox(uint id) {
            return await context.Boxes.FindAsync(id);
        }

        public async Task<ActionResult<Box>> addBox(uint store_id, uint creator_user){

            StoreBackend.Services.Inventories.StoreService store_service = new StoreBackend.Services.Inventories.StoreService(this.context);
            StoreBackend.Services.UserService user_service = new StoreBackend.Services.UserService(this.context);

            User user = await user_service.getUserById(creator_user);
            Store store = await store_service.getStoreById(store_id);

            if(user != null && store != null) {
                
                Box new_box = new Box();
                new_box.UserId = creator_user;
                new_box.StoreId = store_id;

                context.Boxes.Add(new_box);
                var result = await context.SaveChangesAsync();

                return new_box;

            }

            return null;

        }
        
    }
    
}