using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Models;
using StoreBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace StoreBackend.Services.Items
{
    public class ItemService {

        public readonly StoreBackendContext context;

        public ItemService(StoreBackendContext db_context){
            this.context = db_context;
        }

        public async Task<ActionResult<IEnumerable<Item>>> getAll(){
            return await context.Items.ToListAsync();
        }

        public async Task<Item> getItemById(uint id) {
            return await context.Items.FindAsync(id);
        }

        // public async Task<User> getUserByEmail(string email) {
            
        //     User user = context.Users.Where(u => u.Email == email).FirstOrDefault<User>();
        //     return user;

        // }

        public async Task<Item> addItem(string name, double price) {

            Item new_item = new Item();
            new_item.Name = name;
            new_item.Price = price;

            context.Items.Add(new_item);
            var result = await context.SaveChangesAsync();

            return new_item;

        }

        public async Task<Item> changeItem(Item new_item) {
            
            Item item = await this.getItemById(new_item.Id);

            if(item != null) {
            
                if(new_item.Name != null){
                    item.Name = new_item.Name;
                }

                if(new_item.Price != null){
                    item.Price = new_item.Price;
                }

                context.Entry(item).State = EntityState.Modified;

                await context.SaveChangesAsync();

            }

            return item;

        }
        
    }
    
}